using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Services;

namespace Voyagoo.Controllers.TourGuides
{
    [Route("tour-guides/{tourGuideId}/bookings")]
    [ApiController]
    [Authorize]
    public class TourGuideBookingsController(ITourGuideBookingService bookingService) : ControllerBase
    {
        private readonly ITourGuideBookingService _bookingService = bookingService;

        [HttpPost("")]
        public async Task<IActionResult> CreateBooking(
            int tourGuideId,
            [FromBody] CreateTourGuideBookingRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var result = await _bookingService.CreateBookingAsync(tourGuideId, userId, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
