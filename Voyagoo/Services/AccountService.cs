using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Account;
using Voyagoo.Entities;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class AccountService(
        UserManager<ApplicationUser> userManager,
        IImageService imageService,
        VoyagooDbContext context) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IImageService _imageService = imageService;
        private readonly VoyagooDbContext _context = context;

        public async Task<Result<GetProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<GetProfileResponse>(UserErrors.EmailNotFound);

            var response = new GetProfileResponse(
                user.FirstName,
                user.LastName,
                user.Email!,
                user.PhoneNumber,
                user.ProfilePictureUrl
            );

            return Result.Success(response);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.EmailNotFound);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            return Result.Success();
        }

        public async Task<Result<string>> UpdateProfilePictureAsync(string userId, IFormFile image, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<string>(UserErrors.EmailNotFound);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return Result.Failure<string>(UserErrors.InvalidImageFile);

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                await _imageService.DeleteImageAsync(user.ProfilePictureUrl);

            var imageUrl = await _imageService.UploadImageAsync(image, "voyagoo/users", cancellationToken);

            user.ProfilePictureUrl = imageUrl;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure<string>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            return Result.Success(user.ProfilePictureUrl);
        }

        public async Task<Result<GetAllBookingsResponse>> GetAllBookingsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var hotelBookings = await _context.HotelBookings
                .Where(b => b.UserId == userId && !string.IsNullOrEmpty(b.PaymentType))
                .Include(b => b.Hotel).ThenInclude(h => h.Images)
                .OrderByDescending(b => b.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var tourGuideBookings = await _context.TourGuideBookings
                .Where(b => b.UserId == userId && !string.IsNullOrEmpty(b.PaymentType))
                .Include(b => b.TourGuide)
                .OrderByDescending(b => b.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var restaurantBookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Restaurant).ThenInclude(r => r.Images)
                .OrderByDescending(b => b.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = new GetAllBookingsResponse(
                HotelBookings: hotelBookings.Select(b => new HotelBookingHistoryItem(
                    b.Id,
                    b.Hotel.Name,
                    b.CheckIn,
                    b.CheckOut,
                    b.Nights,
                    b.TotalPrice,
                    b.PaymentType,
                    b.Status.ToString(),
                    b.CreatedAt,
                    b.Hotel.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? b.Hotel.Images.FirstOrDefault()?.ImageUrl
                )).ToList(),

                TourGuideBookings: tourGuideBookings.Select(b => new TourGuideBookingHistoryItem(
                    b.Id,
                    b.TourGuide.Name,
                    b.BookingDate,
                    b.NumberOfDays,
                    b.TotalPrice,
                    b.PaymentType,
                    b.Status.ToString(),
                    b.CreatedAt,
                    b.TourGuide.ProfilePictureUrl
                )).ToList(),

                RestaurantBookings: restaurantBookings.Select(b => new RestaurantBookingHistoryItem(
                    b.Id,
                    b.Restaurant.Name,
                    b.Restaurant.Address,
                    b.BookingDate,
                    b.GuestName,
                    b.GuestPhone,
                    b.TablesForTwo,
                    b.TablesForFour,
                    b.TablesForSix,
                    b.CreatedAt,
                    b.Restaurant.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? b.Restaurant.Images.FirstOrDefault()?.ImageUrl
                )).ToList()
            );

            return Result.Success(response);
        }

        public async Task<Result> DeleteBookingAsync(
            string userId,
            int bookingId,
            string bookingType,
            CancellationToken cancellationToken = default)
        {
            if (bookingType.ToLower() == "hotel")
            {
                var booking = await _context.HotelBookings
                    .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId, cancellationToken);

                if (booking is null)
                    return Result.Failure(HotelBookingErrors.BookingNotFound);

                _context.HotelBookings.Remove(booking);
            }
            else if (bookingType.ToLower() == "tourguide")
            {
                var booking = await _context.TourGuideBookings
                    .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId, cancellationToken);

                if (booking is null)
                    return Result.Failure(TourGuideBookingErrors.BookingNotFound);

                _context.TourGuideBookings.Remove(booking);
            }
            else if (bookingType.ToLower() == "restaurant")
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId, cancellationToken);

                if (booking is null)
                    return Result.Failure(new Error(
                        "RestaurantBooking.NotFound",
                        "Booking not found",
                        StatusCodes.Status404NotFound
                    ));

                _context.Bookings.Remove(booking);
            }
            else
            {
                return Result.Failure(new Error(
                    "Booking.InvalidType",
                    "bookingType must be 'hotel', 'tourguide' or 'restaurant'",
                    StatusCodes.Status400BadRequest
                ));
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}