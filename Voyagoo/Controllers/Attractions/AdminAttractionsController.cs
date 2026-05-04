using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Attractions;
using Voyagoo.Entities.Attractions;
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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] AttractionStatus status, CancellationToken cancellationToken)
        {
            var result = await _attractionService.UpdateAttractionStatusAsync(id, status, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            var statuses = Enum.GetValues<AttractionStatus>()
                .Select(s => new { id = (int)s, name = s.ToString() });

            return Ok(statuses);
        }

        [HttpGet("GetAllAttractions")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _attractionService.GetAllAttractionsAdminAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
