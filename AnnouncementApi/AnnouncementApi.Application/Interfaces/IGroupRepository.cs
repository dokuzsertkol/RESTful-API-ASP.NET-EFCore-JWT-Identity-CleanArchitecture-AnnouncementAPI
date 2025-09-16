using AnnouncementApi.Domain.Entities;
using AnnouncementApi.Application.DTOs.Group;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group> CreateAsync(Group group);
        Task<bool> CheckIfExists(int id);
        Task<Group?> UpdateAsync(int id, UpdateGroupDto updateGroupDto);
        Task<Group?> DeleteAsync(int id);
        Task<Group?> GetGroup(int id);
        IQueryable<Group?> GetGroupDetails(int id);
    }
}
