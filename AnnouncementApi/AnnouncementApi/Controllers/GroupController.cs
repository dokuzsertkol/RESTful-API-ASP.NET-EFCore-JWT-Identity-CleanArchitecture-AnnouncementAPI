using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Application.DTOs.User;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Application.Queries;
using AnnouncementApi.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApi.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // group CRUD operations
        // create a group
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto createGroupDto)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null) return Unauthorized(new ApiResponse<object> { Success = false, Message = "Please log in before creating a group." });

            // this automaticaly creates groupuser element
            var groupDto = await _groupService.CreateGroupAsync(createGroupDto, currentUserId);

            return Ok(new ApiResponse<GroupDto> { Data = groupDto, Message = "The group has been created successfully." });
        }

        // read a group and its content
        [HttpGet("{groupId:int}")]
        [Authorize(Policy = "GroupMember")]
        public async Task<IActionResult> GetGroup([FromRoute] int groupId, [FromQuery] GroupQueryObject query)
        {
            var groupDetailDto = await _groupService.GetGroupDetailsAsync(groupId, query);
            if (groupDetailDto == null) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified group could not be found." });

            return Ok(new ApiResponse<GroupDetailDto> { Data = groupDetailDto });
        }

        // update a group
        [HttpPut("{groupId:int}")]
        [Authorize(Policy = "GroupAdmin")]
        public async Task<IActionResult> UpdateGroup([FromRoute] int groupId, [FromBody] UpdateGroupDto updateGroupDto)
        {
            var groupDto = await _groupService.UpdateGroupAsync(groupId, updateGroupDto);
            if (groupDto == null) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified group could not be found." });

            return Ok(new ApiResponse<GroupDto> { Data = groupDto, Message = "The group name has been updated successfully." });
        }

        // delete a group
        [HttpDelete("{groupId:int}")]
        [Authorize(Policy = "GroupAdmin")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int groupId)
        {
            var result = await _groupService.DeleteGroupAsync(groupId);
            if (!result) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified group could not be found." });
            return Ok(new ApiResponse<object> { Message = "The group has been deleted successfully." });
        }
        // ----

        // other group operations
        // read members of a group
        [HttpGet("{groupId:int}/users")]
        [Authorize(Policy = "GroupMember")]
        public async Task<IActionResult> GetUsers([FromRoute] int groupId, [FromQuery] StringQueryObject query)
        {
            var userDtos = await _groupService.GetUsersAsync(groupId, query);
            return Ok(new ApiResponse<IReadOnlyList<AppUserDto>> { Data = userDtos });
        }

        // add a user to a group
        [HttpPost("{groupId:int}/users")]
        [Authorize(Policy = "GroupAdmin")]
        public async Task<IActionResult> AddUserToGroup([FromRoute] int groupId, [FromBody] AddUserToGroupDto addUserToGroupDto)
        {
            var userDto = await _groupService.AddUserToGroupAsync(groupId, addUserToGroupDto.UserId, false);
            if (userDto == null) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified group or user was not found, or the user is already a member." });
            return Ok(new ApiResponse<AppUserDto> { Data = userDto, Message = "The user has been added to the group successfully." });
        }

        // remove a user from a group
        [HttpDelete("{groupId:int}/users/{userId}")]
        [Authorize(Policy = "GroupAdmin")]
        public async Task<IActionResult> RemoveUserFromGroup([FromRoute] int groupId, [FromRoute] string userId)
        {
            var result = await _groupService.RemoveUserFromGroupAsync(groupId, userId);
            if (!result) return NotFound(new ApiResponse<object> { Success = false, Message = "The specified group or member was not found." });
            return Ok(new ApiResponse<AppUserDto> { Message = "The user has been removed from the group successfully." });
        }

        // create announcement to a group
        [HttpPost("{groupId:int}/announcements")]
        [Authorize(Policy = "GroupAdmin")]
        public async Task<IActionResult> AddAnnouncement([FromRoute] int groupId, [FromBody] AddAnnouncementDto addAnnouncementDto)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null) return Unauthorized(new ApiResponse<object> { Success = false, Message = "You are not logged in." });

            var announcement = await _groupService.AddAnnouncementAsync(groupId, addAnnouncementDto, currentUserId);
            if (announcement == null) return NotFound(new ApiResponse<object> { Success = false, Message = "You are not a member of this group." });

            return Ok(new ApiResponse<AnnouncementDto> { Data = announcement, Message = "The announcement has been added successfully." });
        }

        // get {groupId:int}/announcements
        [HttpGet("{groupId:int}/announcements")]
        [Authorize(Policy = "GroupMember")]
        public async Task<IActionResult> GetAnnouncements([FromRoute] int groupId, [FromQuery] StringQueryObject query)
        {
            var announcementDtos = await _groupService.GetAnnouncementsByGroupId(groupId, query);
            return Ok(new ApiResponse<IReadOnlyList<AnnouncementDto>> { Data = announcementDtos });
        }
    }
}
