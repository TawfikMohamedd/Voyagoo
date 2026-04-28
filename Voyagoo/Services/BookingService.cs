using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class BookingService(VoyagooDbContext context) : IBookingService
    {
        private readonly VoyagooDbContext _context = context;

        public async Task<Result<CreateBookingResponse>> CreateBookingAsync(
            int restaurantId,
            string userId,
            CreateBookingRequest request,
            CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == restaurantId && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure<CreateBookingResponse>(RestaurantErrors.RestaurantNotFound);

            var bookingDate = request.BookingDate;

            // جيب الحجوزات الموجودة في نفس اليوم
            var bookedOnSameDay = await _context.Bookings
                .Where(b => b.RestaurantId == restaurantId
                         && b.BookingDate == bookingDate)
                .ToListAsync(cancellationToken);

            // احسب المحجوز
            var bookedForTwo = bookedOnSameDay.Sum(b => b.TablesForTwo);
            var bookedForFour = bookedOnSameDay.Sum(b => b.TablesForFour);
            var bookedForSix = bookedOnSameDay.Sum(b => b.TablesForSix);

            // احسب المتاح
            var availableForTwo = restaurant.TablesForTwo - bookedForTwo;
            var availableForFour = restaurant.TablesForFour - bookedForFour;
            var availableForSix = restaurant.TablesForSix - bookedForSix;

            // تحقق من الـ availability
            if (request.TablesForTwo > availableForTwo)
                return Result.Failure<CreateBookingResponse>(BookingErrors.NotEnoughTablesForTwo);

            if (request.TablesForFour > availableForFour)
                return Result.Failure<CreateBookingResponse>(BookingErrors.NotEnoughTablesForFour);

            if (request.TablesForSix > availableForSix)
                return Result.Failure<CreateBookingResponse>(BookingErrors.NotEnoughTablesForSix);

            // سجل الحجز مباشرة
            var booking = new Booking
            {
                RestaurantId = restaurantId,
                UserId = userId,
                BookingDate = request.BookingDate,
                GuestName = request.GuestName,
                GuestPhone = request.GuestPhone,
                TablesForTwo = request.TablesForTwo,
                TablesForFour = request.TablesForFour,
                TablesForSix = request.TablesForSix
            };

            await _context.Bookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateBookingResponse(
                booking.Id,
                restaurant.Name,
                restaurant.Address,
                booking.BookingDate,
                booking.GuestName,
                booking.GuestPhone,
                booking.TablesForTwo,
                booking.TablesForFour,
                booking.TablesForSix
            );

            return Result.Success(response);
        }
    }
}
