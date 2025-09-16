using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApi.Controllers
{
    [Route("api/announcements")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        // announcement CRUD operations
        // create an announcement
        // located in groupcontroller, Route is /api/group/{groupId}/announcement

        // read an announcement
        [HttpGet("{announcementId:int}")]
        [Authorize(Policy = "AnnouncementReader")]
        public async Task<IActionResult> GetAnnouncement([FromRoute] int announcementId)
        {
            var announcement = await _announcementService.GetAnnouncementAsync(announcementId);

            return Ok(new ApiResponse<AnnouncementDto> { Data = announcement });

        }

        // update an announcement
        [HttpPut("{announcementId:int}")]
        [Authorize(Policy = "AnnouncementCreator")]
        public async Task<IActionResult> UpdateAnnouncement([FromRoute] int announcementId, [FromBody] UpdateAnnouncementDto updateAnnouncementDto)
        {
            var announcementDto = await _announcementService.UpdateAnnouncementAsync(announcementId, updateAnnouncementDto);
            if (announcementDto == null) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified announcement could not be found." });

            return Ok(new ApiResponse<AnnouncementDto> { Data = announcementDto, Message = "The announcement has been updated successfully." });
        }

        // delete an announcement
        [HttpDelete("{announcementId:int}")]
        [Authorize(Policy = "AnnouncementCreator")]
        public async Task<IActionResult> DeleteAnnouncement([FromRoute] int announcementId)
        {
            var result = await _announcementService.DeleteAnnouncementAsync(announcementId);
            if (!result) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified announcement could not be found." });
            return Ok(new ApiResponse<object> { Message = "The announcement has been deleted successfully." });
        }
    }
}
