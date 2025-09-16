using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Application.Queries;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Application.Mappers;
using AnnouncementApi.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace AnnouncementApi.Infrastructure.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IGroupUserRepository _groupUserRepo;
        private readonly IAnnouncementRepository _announcementRepo;

        public AppUserService(IGroupUserRepository groupUserRepository, IAnnouncementRepository announcementRepository)
        {
            _groupUserRepo = groupUserRepository;
            _announcementRepo = announcementRepository;
        }

        public async Task<GroupUser?> LeaveGroupAsync(int groupId, string userId)
        {
            var groupUser = await _groupUserRepo.GetGroupUser(groupId, userId);
            if (groupUser == null) return null;

            return await _groupUserRepo.RemoveUserFromGroup(groupId, userId);
        }

        public async Task<IReadOnlyList<GroupDto>> GetGroupsByUserIdAsync(string userId, StringQueryObject query)
        {
            query ??= new StringQueryObject();

            var groupsQuery = _groupUserRepo.GetGroups(userId);

            if (!string.IsNullOrWhiteSpace(query.SearchFor))
                groupsQuery = groupsQuery.Where(x => x.Name.ToLower().Contains(query.SearchFor.ToLower()));

            var groups = groupsQuery.OrderBy(x => x.Name);

            return await groups
                .Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .Select(x => x.GroupToGroupDto())
                .ToListAsync();
        }
        
        public async Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByUserIdAsync(string userId, StringQueryObject query)
        {
            query ??= new StringQueryObject();

            return await _announcementRepo.GetAnnouncementsByUserId(query, userId);
        }
    }
}
