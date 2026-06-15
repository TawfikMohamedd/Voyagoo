using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Hotels
{
    [Route("admin/booking-features")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminBookingFeaturesController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        [HttpGet("")]
        public async Task<IActionResult> GetAllBookingFeatures(CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetAllBookingFeaturesAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBookingFeature([FromBody] AddBookingFeatureRequest request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.AddBookingFeatureAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
