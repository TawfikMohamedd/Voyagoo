using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Restaurants
{
    [Route("admin/features")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminFeaturesController(IRestaurantService restaurantService) : ControllerBase
    {
        private readonly IRestaurantService _restaurantService = restaurantService;

        [HttpGet("")]
        public async Task<IActionResult> GetAllFeatures(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetAllFeaturesAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddFeature([FromBody] AddFeatureRequest request, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.AddFeatureAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
