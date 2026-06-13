using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Hotels
{
    [Route("[controller]")]
    [ApiController]
    public class HotelsController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetAllHotelsAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetHotelByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("{id}/comments")]
        [Authorize]
        public async Task<IActionResult> AddComment(int id, [FromBody] AddHotelCommentRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var result = await _hotelService.AddCommentAsync(id, userId, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}/comments/{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id, int commentId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var result = await _hotelService.DeleteOwnCommentAsync(id, commentId, userId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}