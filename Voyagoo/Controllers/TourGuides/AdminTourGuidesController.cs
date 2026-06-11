using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Common;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Entities.TourGuides;
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTourGuide(
            int id,
            [FromBody] UpdateTourGuideRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _tourGuideService.UpdateTourGuideAsync(id, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<TourGuideStatus>(request.Status, true, out var parsedStatus) || !Enum.IsDefined(typeof(TourGuideStatus), parsedStatus))
                return BadRequest("Invalid status value");

            var result = await _tourGuideService.UpdateTourGuideStatusAsync(id, parsedStatus, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            var statuses = Enum.GetValues<TourGuideStatus>()
                .Select(s => new { id = (int)s, name = s.ToString() });

            return Ok(statuses);
        }

        [HttpGet("GetAllTourGuides")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _tourGuideService.GetAllTourGuidesAdminAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}