using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Hotels
{
    [Route("admin/hotel-features")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminHotelFeaturesController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        [HttpGet("")]
        public async Task<IActionResult> GetAllFeatures(CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetAllHotelFeaturesAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddFeature([FromBody] AddHotelFeatureRequest request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.AddHotelFeatureAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}