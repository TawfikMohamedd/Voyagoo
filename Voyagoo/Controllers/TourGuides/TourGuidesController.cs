using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers.TourGuides
{
    [Route("[controller]")]
    [ApiController]
    public class TourGuidesController(ITourGuideService tourGuideService) : ControllerBase
    {
        private readonly ITourGuideService _tourGuideService = tourGuideService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _tourGuideService.GetAllTourGuidesAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _tourGuideService.GetTourGuideByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
