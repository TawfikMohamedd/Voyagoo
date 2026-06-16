using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Entities.Hotels;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class HotelBookingService(VoyagooDbContext context) : IHotelBookingService
    {
        private readonly VoyagooDbContext _context = context;

        public async Task<Result<CreateHotelBookingResponse>> CreateBookingAsync(
            int hotelId,
            string userId,
            CreateHotelBookingRequest request,
            CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .Where(h => h.Id == hotelId && !h.IsDeleted)
                .Include(h => h.BookingFeatures).ThenInclude(bf => bf.BookingFeature)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (hotel is null)
                return Result.Failure<CreateHotelBookingResponse>(HotelErrors.HotelNotFound);

            // ── تأكد إن كل نوع غرفة مختار موجود فعلاً في الفندق (سعره > 0) ──
            foreach (var roomSelection in request.Rooms)
            {
                var (_, price) = GetRoomTypeInfo(hotel, (RoomType)roomSelection.RoomType);
                if (price <= 0)
                    return Result.Failure<CreateHotelBookingResponse>(HotelBookingErrors.InvalidRoomType);
            }

            // ── تأكد إن كل Extra Feature موجود في الفندق ──
            var hotelFeatureIds = hotel.BookingFeatures.Select(bf => bf.BookingFeatureId).ToHashSet();
            foreach (var extra in request.ExtraFeatures)
            {
                if (!hotelFeatureIds.Contains(extra.BookingFeatureId))
                    return Result.Failure<CreateHotelBookingResponse>(HotelBookingErrors.InvalidFeature);
            }

            var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;

            // ── حساب الغرف ──
            var bookingRooms = new List<HotelBookingRoom>();
            var roomItems = new List<BookedRoomItem>();
            decimal roomsTotal = 0;

            foreach (var roomSelection in request.Rooms)
            {
                var roomType = (RoomType)roomSelection.RoomType;
                var (_, pricePerNight) = GetRoomTypeInfo(hotel, roomType);
                var total = pricePerNight * nights * roomSelection.Quantity;
                roomsTotal += total;

                bookingRooms.Add(new HotelBookingRoom
                {
                    RoomType = roomType,
                    Quantity = roomSelection.Quantity,
                    PricePerNight = pricePerNight
                });

                roomItems.Add(new BookedRoomItem(
                    roomType.ToString(),
                    roomSelection.Quantity,
                    pricePerNight,
                    total
                ));
            }

            // ── حساب البورد (Full/Half) ──
            var bookingFeatures = new List<HotelBookingFeatureSelection>();
            var featureItems = new List<BookedFeatureItem>();
            decimal boardsTotal = 0;

            if (request.FullBoardRooms > 0)
            {
                var fullBoard = hotel.BookingFeatures
                    .First(bf => bf.BookingFeatureId == DefaultBookingFeatures.FullBoardId);
                var total = fullBoard.Price * nights * request.FullBoardRooms;
                boardsTotal += total;

                bookingFeatures.Add(new HotelBookingFeatureSelection
                {
                    BookingFeatureId = DefaultBookingFeatures.FullBoardId,
                    RoomsCount = request.FullBoardRooms,
                    PricePerNight = fullBoard.Price
                });

                featureItems.Add(new BookedFeatureItem(
                    DefaultBookingFeatures.FullBoardId,
                    fullBoard.BookingFeature.Name,
                    fullBoard.BookingFeature.Icon,
                    request.FullBoardRooms,
                    fullBoard.Price,
                    total
                ));
            }

            if (request.HalfBoardRooms > 0)
            {
                var halfBoard = hotel.BookingFeatures
                    .First(bf => bf.BookingFeatureId == DefaultBookingFeatures.HalfBoardId);
                var total = halfBoard.Price * nights * request.HalfBoardRooms;
                boardsTotal += total;

                bookingFeatures.Add(new HotelBookingFeatureSelection
                {
                    BookingFeatureId = DefaultBookingFeatures.HalfBoardId,
                    RoomsCount = request.HalfBoardRooms,
                    PricePerNight = halfBoard.Price
                });

                featureItems.Add(new BookedFeatureItem(
                    DefaultBookingFeatures.HalfBoardId,
                    halfBoard.BookingFeature.Name,
                    halfBoard.BookingFeature.Icon,
                    request.HalfBoardRooms,
                    halfBoard.Price,
                    total
                ));
            }

            // ── حساب الفيتشرز الإضافية ──
            decimal extrasTotal = 0;

            foreach (var extra in request.ExtraFeatures)
            {
                var feature = hotel.BookingFeatures
                    .First(bf => bf.BookingFeatureId == extra.BookingFeatureId);
                var total = feature.Price * nights * extra.RoomsCount;
                extrasTotal += total;

                bookingFeatures.Add(new HotelBookingFeatureSelection
                {
                    BookingFeatureId = extra.BookingFeatureId,
                    RoomsCount = extra.RoomsCount,
                    PricePerNight = feature.Price
                });

                featureItems.Add(new BookedFeatureItem(
                    extra.BookingFeatureId,
                    feature.BookingFeature.Name,
                    feature.BookingFeature.Icon,
                    extra.RoomsCount,
                    feature.Price,
                    total
                ));
            }

            // ── الحساب النهائي ──
            var subtotal = roomsTotal + boardsTotal + extrasTotal;
            var discountAmount = subtotal * (hotel.Discount / 100);
            var afterDiscount = subtotal - discountAmount;
            var serviceChargeAmount = afterDiscount * (hotel.ServiceCharge / 100);
            var totalPrice = afterDiscount + serviceChargeAmount;

            var booking = new HotelBooking
            {
                HotelId = hotelId,
                UserId = userId,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                Nights = nights,
                RoomsTotal = roomsTotal,
                BoardsTotal = boardsTotal,
                ExtrasTotal = extrasTotal,
                Subtotal = subtotal,
                DiscountPercentage = hotel.Discount,
                DiscountAmount = discountAmount,
                ServiceChargePercentage = hotel.ServiceCharge,
                ServiceChargeAmount = serviceChargeAmount,
                TotalPrice = totalPrice,
                Rooms = bookingRooms,
                SelectedFeatures = bookingFeatures
            };

            await _context.HotelBookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateHotelBookingResponse(
                booking.Id,
                hotel.Name,
                booking.CheckIn,
                booking.CheckOut,
                booking.Nights,
                roomItems,
                featureItems,
                roomsTotal,
                boardsTotal,
                extrasTotal,
                subtotal,
                hotel.Discount,
                discountAmount,
                hotel.ServiceCharge,
                serviceChargeAmount,
                totalPrice
            ));
        }

        private static (int Count, decimal Price) GetRoomTypeInfo(Hotel hotel, RoomType roomType) => roomType switch
        {
            RoomType.Single => (hotel.SingleRooms, hotel.SinglePrice),
            RoomType.Double => (hotel.DoubleRooms, hotel.DoublePrice),
            RoomType.Triple => (hotel.TripleRooms, hotel.TriplePrice),
            RoomType.Suite => (hotel.SuiteRooms, hotel.SuitePrice),
            _ => (0, 0)
        };
    }
}