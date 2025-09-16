using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Application.Queries;
using AnnouncementApi.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        // read (list) current user's groups
        [HttpGet("/me/groups")]
        [Authorize]
        public async Task<IActionResult> GetGroupsByUserId([FromQuery] StringQueryObject query)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new ApiResponse<object> { Success = false, Message = "You are not logged in." });

            var groupDtos = await _appUserService.GetGroupsByUserIdAsync(userId, query);

            return Ok(new ApiResponse<IReadOnlyList<GroupDto>> { Data = groupDtos });
        }

        // leave a joined group
        [HttpDelete("/me/groups/{groupId:int}")]
        [Authorize(Policy = "GroupMember")]
        public async Task<IActionResult> LeaveGroup([FromRoute] int groupId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new ApiResponse<object> { Success = false, Message = "You are not logged in." });

            var groupUser = await _appUserService.LeaveGroupAsync(groupId, userId);
            if (groupUser == null) return NotFound(new ApiResponse<object> { Success = false, Message = "You are not a member of this group." });
            return Ok(new ApiResponse<object> { Message = "Left the group successfully." });
        }

        // read (list) current user's groups
        [HttpGet("/me/announcements")]
        [Authorize]
        public async Task<IActionResult> GetAnnouncementsByUserId([FromQuery] StringQueryObject query)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new ApiResponse<object> { Success = false, Message = "You are not logged in." });

            var announcementDtos = await _appUserService.GetAnnouncementsByUserIdAsync(userId, query);

            return Ok(new ApiResponse<IReadOnlyList<AnnouncementDto>> { Data = announcementDtos });
        }
    }
}
