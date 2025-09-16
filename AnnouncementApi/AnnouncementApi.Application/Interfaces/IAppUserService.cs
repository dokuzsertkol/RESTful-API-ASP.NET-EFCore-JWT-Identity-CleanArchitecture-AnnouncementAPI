using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Application.Queries;
using AnnouncementApi.Domain.Entities;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IAppUserService
    {
        Task<GroupUser?> LeaveGroupAsync(int groupId, string userId);
        Task<IReadOnlyList<GroupDto>> GetGroupsByUserIdAsync(string userId, StringQueryObject query);
        Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByUserIdAsync(string userId, StringQueryObject query);
    }
}
