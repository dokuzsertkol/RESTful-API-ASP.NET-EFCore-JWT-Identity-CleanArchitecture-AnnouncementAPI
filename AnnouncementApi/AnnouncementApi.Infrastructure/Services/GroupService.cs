using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Application.DTOs.User;
using AnnouncementApi.Application.Queries;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Application.Mappers;
using AnnouncementApi.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AnnouncementApi.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepo;
        private readonly IGroupUserRepository _groupUserRepo;
        private readonly IAnnouncementRepository _announcementRepo;
        private readonly UserManager<AppUser> _userManager;

        public GroupService(IGroupRepository groupRepo, IGroupUserRepository groupUserRepo, IAnnouncementRepository announcementRepo, UserManager<AppUser> userManager)
        {
            _groupRepo = groupRepo;
            _groupUserRepo = groupUserRepo;
            _announcementRepo = announcementRepo;
            _userManager = userManager;
        }

        public async Task<GroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, string userId)
        {
            var group = createGroupDto.CreateGroupDtoToGroup();

            await _groupRepo.CreateAsync(group);

            GroupUser groupUser = new()
            {
                UserId = userId,
                GroupId = group.Id,
                IsAdmin = true
            };

            await _groupUserRepo.AddUserToGroup(groupUser);

            return group.GroupToGroupDto();
        }

        public async Task<GroupDetailDto?> GetGroupDetailsAsync(int groupId, GroupQueryObject query)
        {
            query ??= new GroupQueryObject();

            int userSkip = (query.UserPageNumber - 1) * query.UserPageSize;
            int announcementSkip = (query.AnnouncementPageNumber - 1) * query.AnnouncementPageSize;

            var groupQuery = _groupRepo.GetGroupDetails(groupId);
            var group = await groupQuery.FirstOrDefaultAsync();
            if (group == null) return null;

            var pagedAnnouncements = group.Announcements
                .Where(x => string.IsNullOrEmpty(query.SearchFor)
                            || x.Title.ToLower().Contains(query.SearchFor.ToLower())
                            || x.Content.ToLower().Contains(query.SearchFor.ToLower()))
                .OrderByDescending(x => x.LastUpdated)
                .Skip(announcementSkip)
                .Take(query.AnnouncementPageSize)
                .ToList();

            var pagedUsers = group.GroupUsers
                .OrderBy(x => x.User.UserName)
                .Skip(userSkip)
                .Take(query.UserPageSize)
                .ToList();

            group.Announcements = pagedAnnouncements;
            group.GroupUsers = pagedUsers;

            return group.GroupToGroupDetailDto();
        }

        public async Task<GroupDto?> UpdateGroupAsync(int groupId, UpdateGroupDto updateGroupDto)
        {
            var group = await _groupRepo.UpdateAsync(groupId, updateGroupDto);
            if (group == null) return null;

            return group.GroupToGroupDto();
        }

        public async Task<bool> DeleteGroupAsync(int groupId)
        {
            var group = await _groupRepo.GetGroup(groupId);
            if (group == null) return false;

            await _groupRepo.DeleteAsync(groupId);

            return true;
        }

        public async Task<IReadOnlyList<AppUserDto>> GetUsersAsync(int groupId, StringQueryObject query)
        {
            query ??= new StringQueryObject();

            var usersQuery = _groupUserRepo.GetUsers(groupId);

            if (!string.IsNullOrWhiteSpace(query.SearchFor))
                usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(query.SearchFor.ToLower()));

            var userDtos = await usersQuery
                .OrderBy(x => x.UserName)
                .Select(x => x.UserToUserDto())
                .ToListAsync();

            return userDtos;
        }

        public async Task<AppUserDto?> AddUserToGroupAsync(int groupId, string userId, bool isAdmin)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null) return null;

            GroupUser groupUser = new()
            {
                UserId = userId,
                GroupId = groupId,
                IsAdmin = isAdmin
            };

            var result = await _groupUserRepo.AddUserToGroup(groupUser);
            if (result == null) return null;

            return result.User.UserToUserDto();
        }
        
        public async Task<bool> RemoveUserFromGroupAsync(int groupId, string userId)
        {
            var groupUser = await _groupUserRepo.RemoveUserFromGroup(groupId, userId);
            if (groupUser == null) return false;

            return true;
        }

        public async Task<AnnouncementDto?> AddAnnouncementAsync(int groupId, AddAnnouncementDto addAnnouncementDto, string userId)
        {
            if (!await _groupRepo.CheckIfExists(groupId)) return null;

            var announcement = await _announcementRepo.AddAnnouncement(groupId, addAnnouncementDto, userId);
            if (announcement == null) return null;

            return announcement.AnnouncementToAnnouncementDto();
        }

        public async Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByGroupId(int groupId, StringQueryObject query)
        {
            query ??= new StringQueryObject();

            return await _announcementRepo.GetAnnouncementsByGroupId(query, groupId);
        }
    }
}
