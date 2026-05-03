using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Attractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Attractions
{
    [Route("admin/attractions")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAttractionsController(IAttractionService attractionService) : ControllerBase
    {
        private readonly IAttractionService _attractionService = attractionService;

        [HttpPost("")]
        public async Task<IActionResult> AddAttraction([FromBody] AddAttractionRequest request, CancellationToken cancellationToken)
        {
            var result = await _attractionService.AddAttractionAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddImages(int id, [FromForm] List<IFormFile> images, CancellationToken cancellationToken)
        {
            if (images is null || images.Count == 0)
                return BadRequest("No images provided");

            var result = await _attractionService.AddAttractionImagesAsync(id, images, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttraction(int id, [FromBody] UpdateAttractionRequest request, CancellationToken cancellationToken)
        {
            var result = await _attractionService.UpdateAttractionAsync(id, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttraction(int id, CancellationToken cancellationToken)
        {
            var result = await _attractionService.DeleteAttractionAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int id, int imageId, CancellationToken cancellationToken)
        {
            var result = await _attractionService.DeleteAttractionImageAsync(id, imageId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
