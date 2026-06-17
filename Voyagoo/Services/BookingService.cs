using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Entities;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services;

public class BookingService(
    VoyagooDbContext context,
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender) : IBookingService
{
    private readonly VoyagooDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result<CreateBookingResponse>> CreateBookingAsync(
        int restaurantId,
        string userId,
        CreateBookingRequest request,
        CancellationToken cancellationToken = default)
    {
        var restaurant = await _context.Restaurants
            .FirstOrDefaultAsync(
                r => r.Id == restaurantId && !r.IsDeleted,
                cancellationToken);

        if (restaurant is null)
            return Result.Failure<CreateBookingResponse>(
                RestaurantErrors.RestaurantNotFound);

        var bookingDate = request.BookingDate;

        var bookedOnSameDay = await _context.Bookings
            .Where(b => b.RestaurantId == restaurantId
                     && b.BookingDate == bookingDate)
            .ToListAsync(cancellationToken);

        var bookedForTwo = bookedOnSameDay.Sum(b => b.TablesForTwo);
        var bookedForFour = bookedOnSameDay.Sum(b => b.TablesForFour);
        var bookedForSix = bookedOnSameDay.Sum(b => b.TablesForSix);

        var availableForTwo = restaurant.TablesForTwo - bookedForTwo;
        var availableForFour = restaurant.TablesForFour - bookedForFour;
        var availableForSix = restaurant.TablesForSix - bookedForSix;

        if (request.TablesForTwo > availableForTwo)
            return Result.Failure<CreateBookingResponse>(
                BookingErrors.NotEnoughTablesForTwo);

        if (request.TablesForFour > availableForFour)
            return Result.Failure<CreateBookingResponse>(
                BookingErrors.NotEnoughTablesForFour);

        if (request.TablesForSix > availableForSix)
            return Result.Failure<CreateBookingResponse>(
                BookingErrors.NotEnoughTablesForSix);

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

        // ── Send Confirmation Email ──
        var user = await _userManager.FindByIdAsync(userId);

        if (user is not null && !string.IsNullOrWhiteSpace(user.Email))
        {
            var emailBody =
                EmailTemplates.GetRestaurantBookingConfirmationTemplate(
                    user.FirstName,
                    response);

            await _emailSender.SendEmailAsync(
                user.Email,
                $"Voyagoo - Restaurant Booking Confirmation #{booking.Id}",
                emailBody);
        }

        return Result.Success(response);
    }
}