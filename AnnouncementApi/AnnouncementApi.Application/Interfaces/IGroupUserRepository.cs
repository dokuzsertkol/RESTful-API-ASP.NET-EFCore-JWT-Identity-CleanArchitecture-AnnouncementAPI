using AnnouncementApi.Domain.Entities;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IGroupUserRepository
    {
        Task<GroupUser?> AddUserToGroup(GroupUser groupUser);
        Task<GroupUser?> GetGroupUser(int groupId, string userId);
        Task<GroupUser?> RemoveUserFromGroup(int groupId, string userId);
        Task DeleteAsync(int groupId);
        IQueryable<AppUser> GetUsers(int groupId);
        IQueryable<Group> GetGroups(string userId);
    }
}
