using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Application.DTOs.User;
using AnnouncementApi.Application.Queries;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IGroupService
    {

        Task<GroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, string userId);
        Task<GroupDetailDto?> GetGroupDetailsAsync(int groupId, GroupQueryObject query);
        Task<GroupDto?> UpdateGroupAsync(int groupId, UpdateGroupDto updateGroupDto);
        Task<bool> DeleteGroupAsync(int groupId);
        Task<IReadOnlyList<AppUserDto>> GetUsersAsync(int groupId, StringQueryObject query);
        Task<AppUserDto?> AddUserToGroupAsync(int groupId, string userId, bool isAdmin);
        Task<bool> RemoveUserFromGroupAsync(int groupId, string userId);

        Task<AnnouncementDto?> AddAnnouncementAsync(int groupId, AddAnnouncementDto addAnnouncementDto, string userId);
        Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByGroupId(int groupId, StringQueryObject query);
    }
}
