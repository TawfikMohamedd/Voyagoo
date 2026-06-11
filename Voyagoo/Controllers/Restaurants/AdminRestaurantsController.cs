using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Services;
using Voyagoo.Contracts.Common;

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

        [HttpDelete("{id}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int id, int imageId, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.DeleteRestaurantImageAsync(id, imageId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }





        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<RestaurantStatus>(request.Status, true, out var parsedStatus) || !Enum.IsDefined(typeof(RestaurantStatus), parsedStatus))
                return BadRequest("Invalid status value");

            var result = await _restaurantService.UpdateRestaurantStatusAsync(id, parsedStatus, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            var statuses = Enum.GetValues<RestaurantStatus>()
                .Select(s => new { id = (int)s, name = s.ToString() });

            return Ok(statuses);
        }

        [HttpGet("GetAllRestaurants")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetAllRestaurantsAdminAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}/GetAllComments")]
        public async Task<IActionResult> GetComments(int id, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.GetRestaurantCommentsAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}/comments/{commentId}/DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id, int commentId, CancellationToken cancellationToken)
        {
            var result = await _restaurantService.DeleteCommentAsync(id, commentId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
