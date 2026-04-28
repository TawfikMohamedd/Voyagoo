using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Restaurants
{
    [Route("restaurants/{restaurantId}/bookings")]
    [ApiController]
    [Authorize]
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [HttpPost("")]
        public async Task<IActionResult> CreateBooking(
            int restaurantId,
            [FromBody] CreateBookingRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var result = await _bookingService.CreateBookingAsync(restaurantId, userId, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
