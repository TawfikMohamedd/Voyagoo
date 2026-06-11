using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Services;

namespace Voyagoo.Controllers
{
    [Route("admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUsersAdminAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
