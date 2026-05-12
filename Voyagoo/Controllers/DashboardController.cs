using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers
{
    [Route("admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [HttpGet("")]
        public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
        {
            var result = await _dashboardService.GetDashboardAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
