using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController(IHomeService homeService) : ControllerBase
    {
        private readonly IHomeService _homeService = homeService;

        [HttpGet("")]
        public async Task<IActionResult> GetHome(CancellationToken cancellationToken)
        {
            var result = await _homeService.GetHomeAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}