using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Services;

namespace Voyagoo.Controllers.TourGuides
{
    [Route("admin/tour-guides")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminTourGuidesController(ITourGuideService tourGuideService) : ControllerBase
    {
        private readonly ITourGuideService _tourGuideService = tourGuideService;

        [HttpPost("")]
        public async Task<IActionResult> AddTourGuide([FromBody] AddTourGuideRequest request, CancellationToken cancellationToken)
        {
            var result = await _tourGuideService.AddTourGuideAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> AddImage(int id, IFormFile image, CancellationToken cancellationToken)
        {
            if (image is null)
                return BadRequest("No image provided");

            var result = await _tourGuideService.AddTourGuideImageAsync(id, image, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourGuide(int id, CancellationToken cancellationToken)
        {
            var result = await _tourGuideService.DeleteTourGuideAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("languages")]
        public IActionResult GetLanguages()
        {
            return Ok(_tourGuideService.GetAllLanguages());
        }
    }
}