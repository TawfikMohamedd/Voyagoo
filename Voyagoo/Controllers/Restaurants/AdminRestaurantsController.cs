using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Restaurants
{
    [Route("admin/restaurants")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminRestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        private readonly IRestaurantService _restaurantService = restaurantService;




        [HttpPost("")]
        public async Task<IActionResult> AddRestaurant([FromBody] AddRestaurantRequest request, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.AddRestaurantAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }




        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddImages(int id, [FromForm] List<IFormFile> images, CancellationToken cancellationToken)
        {
            if (images is null || images.Count == 0)
                return BadRequest("No images provided");

            var result = await _restaurantService.AddRestaurantImagesAsync(id, images, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id,[FromBody] UpdateRestaurantRequest request,CancellationToken cancellationToken)
        {
            var result = await _restaurantService.UpdateRestaurantAsync(id, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("cuisine-types")]
        public IActionResult GetCuisineTypes()
        {
            var types = Enum.GetValues<CuisineType>()
                .Select(c => new { id = (int)c, name = c.ToString() });

            return Ok(types);
        }


    }
}
