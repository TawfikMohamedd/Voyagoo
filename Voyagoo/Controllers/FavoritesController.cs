using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voyagoo.Abstractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController(IFavoriteService favoriteService) : ControllerBase
    {
        private readonly IFavoriteService _favoriteService = favoriteService;


        [HttpPost("restaurants/{restaurantId}/toggle")]
        public async Task<IActionResult> ToggleRestaurantFavorite(int restaurantId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _favoriteService.ToggleFavoriteAsync(userId, restaurantId, null, null, cancellationToken);
            return result.IsSuccess ? Ok(new { isFavorited = result.Value }) : result.ToProblem();
        }

        [HttpPost("tour-guides/{tourGuideId}/toggle")]
        public async Task<IActionResult> ToggleTourGuideFavorite(int tourGuideId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _favoriteService.ToggleFavoriteAsync(userId, null, tourGuideId, null, cancellationToken);
            return result.IsSuccess ? Ok(new { isFavorited = result.Value }) : result.ToProblem();
        }

        [HttpPost("attractions/{attractionId}/toggle")]
        public async Task<IActionResult> ToggleAttractionFavorite(int attractionId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _favoriteService.ToggleFavoriteAsync(userId, null, null, attractionId, cancellationToken);
            return result.IsSuccess ? Ok(new { isFavorited = result.Value }) : result.ToProblem();
        }

        [HttpGet("GetAllFavorites")]
        public async Task<IActionResult> GetFavorites(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _favoriteService.GetFavoritesAsync(userId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


    }
}
