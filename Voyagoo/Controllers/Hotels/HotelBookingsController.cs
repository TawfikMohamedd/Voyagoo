using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Hotels
{
    [Route("hotels/{hotelId}/bookings")]
    [ApiController]
    [Authorize]
    public class HotelBookingsController(IHotelBookingService bookingService) : ControllerBase
    {
        private readonly IHotelBookingService _bookingService = bookingService;

        [HttpPost("")]
        public async Task<IActionResult> CreateBooking(
            int hotelId,
            [FromBody] CreateHotelBookingRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _bookingService.CreateBookingAsync(hotelId, userId, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }




        [HttpPost("{bookingId}/confirm")]
        public async Task<IActionResult> ConfirmBooking(int hotelId,int bookingId,[FromBody] ConfirmHotelBookingRequest request,CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _bookingService.ConfirmBookingAsync(bookingId, userId, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetBookingHistory(int hotelId,CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _bookingService.GetBookingHistoryAsync(userId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}