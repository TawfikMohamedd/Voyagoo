using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Common;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Entities.Hotels;
using Voyagoo.Services;

namespace Voyagoo.Controllers.Hotels
{
    [Route("admin/hotels")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminHotelsController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        [HttpPost("")]
        public async Task<IActionResult> AddHotel([FromBody] AddHotelRequest request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.AddHotelAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddImages(int id, [FromForm] List<IFormFile> images, CancellationToken cancellationToken)
        {
            if (images is null || images.Count == 0)
                return BadRequest("No images provided");

            var result = await _hotelService.AddHotelImagesAsync(id, images, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelRequest request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.UpdateHotelAsync(id, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id, CancellationToken cancellationToken)
        {
            var result = await _hotelService.DeleteHotelAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int id, int imageId, CancellationToken cancellationToken)
        {
            var result = await _hotelService.DeleteHotelImageAsync(id, imageId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<HotelStatus>(request.Status, true, out var parsedStatus) || !Enum.IsDefined(typeof(HotelStatus), parsedStatus))
                return BadRequest("Invalid status value");

            var result = await _hotelService.UpdateHotelStatusAsync(id, parsedStatus, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            var statuses = Enum.GetValues<HotelStatus>()
                .Select(s => new { id = (int)s, name = s.ToString() });

            return Ok(statuses);
        }

        [HttpGet("GetAllHotels")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetAllHotelsAdminAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetHotelByIdAdminAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}/GetAllComments")]
        public async Task<IActionResult> GetComments(int id, CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetHotelCommentsAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}/comments/{commentId}/DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id, int commentId, CancellationToken cancellationToken)
        {
            var result = await _hotelService.DeleteCommentAsync(id, commentId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
