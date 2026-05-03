using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Attractions
{
    [Route("[controller]")]
    [ApiController]
    public class AttractionsController(IAttractionService attractionService) : ControllerBase
    {
        private readonly IAttractionService _attractionService = attractionService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _attractionService.GetAllAttractionsAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _attractionService.GetAttractionByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
