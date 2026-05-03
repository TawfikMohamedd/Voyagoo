using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Entities.TourGuides;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class TourGuideBookingService(VoyagooDbContext context) : ITourGuideBookingService
    {
        private readonly VoyagooDbContext _context = context;

        public async Task<Result<CreateTourGuideBookingResponse>> CreateBookingAsync(
            int tourGuideId,
            string userId,
            CreateTourGuideBookingRequest request,
            CancellationToken cancellationToken = default)
        {
            var tourGuide = await _context.TourGuides
                .FirstOrDefaultAsync(g => g.Id == tourGuideId && !g.IsDeleted, cancellationToken);

            if (tourGuide is null)
                return Result.Failure<CreateTourGuideBookingResponse>(TourGuideErrors.TourGuideNotFound);

            // حساب الـ date range للحجز الجديد
            var requestedStart = request.BookingDate;
            var requestedEnd = request.BookingDate.AddDays(request.NumberOfDays - 1);

            // جيب الحجوزات اللي بتتعارض مع الـ date range ده
            var hasConflict = await _context.TourGuideBookings
                .AnyAsync(b => b.TourGuideId == tourGuideId
                    && b.BookingDate <= requestedEnd
                    && b.BookingDate.AddDays(b.NumberOfDays - 1) >= requestedStart,
                    cancellationToken);

            if (hasConflict)
                return Result.Failure<CreateTourGuideBookingResponse>(
                    TourGuideBookingErrors.TourGuideNotAvailable);

            var totalPrice = tourGuide.PricePerDay * request.NumberOfDays;

            var booking = new TourGuideBooking
            {
                TourGuideId = tourGuideId,
                UserId = userId,
                BookingDate = request.BookingDate,
                NumberOfDays = request.NumberOfDays,
                TotalPrice = totalPrice
            };

            await _context.TourGuideBookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateTourGuideBookingResponse(
                booking.Id,
                tourGuide.Name,
                booking.BookingDate,
                booking.NumberOfDays,
                tourGuide.PricePerDay,
                booking.TotalPrice
            );

            return Result.Success(response);
        }
    }
}
