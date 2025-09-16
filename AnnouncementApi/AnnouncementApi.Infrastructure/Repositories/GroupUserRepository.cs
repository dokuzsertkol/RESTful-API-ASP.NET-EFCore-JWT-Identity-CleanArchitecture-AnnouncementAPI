using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AnnouncementApi.Application.Interfaces;

namespace AnnouncementApi.Repositories
{
    public class GroupUserRepository : IGroupUserRepository
    {
        private readonly AppDbContext _context;

        public GroupUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GroupUser?> AddUserToGroup(GroupUser groupUser)
        {
            await _context.GroupUsers.AddAsync(groupUser);
            await _context.SaveChangesAsync();

            var addedGroupUser = await _context.GroupUsers
                .Include(gu => gu.User)
                .Include(gu => gu.Group)
                .FirstOrDefaultAsync(gu => gu.UserId == groupUser.UserId && gu.GroupId == groupUser.GroupId);

            return addedGroupUser;
        }

        public async Task<GroupUser?> GetGroupUser(int groupId, string userId)
        {
            var groupUser = await _context.GroupUsers.Where(x => x.GroupId == groupId && x.UserId == userId).FirstOrDefaultAsync();
            if (groupUser == null) return null;
            return groupUser;
        }

        public async Task<GroupUser?> RemoveUserFromGroup(int groupId, string userId)
        {
            var groupUser = await _context.GroupUsers.Where(x => x.GroupId == groupId && x.UserId == userId).FirstOrDefaultAsync();
            if (groupUser == null) return null;

            _context.GroupUsers.Remove(groupUser);
            await _context.SaveChangesAsync();

            return groupUser;
        }

        public async Task DeleteAsync(int id)
        {
            var groupuser = await _context.GroupUsers.FirstOrDefaultAsync(x => x.GroupId == id);

            if (groupuser == null) return;

            _context.GroupUsers.Remove(groupuser);
            await _context.SaveChangesAsync();
        }

        public IQueryable<AppUser> GetUsers(int groupId)
        {
            return _context.GroupUsers
                .Where(x => x.GroupId == groupId)
                .Select(x => x.User);
        }

        public IQueryable<Group> GetGroups(string userId)
        {
            return _context.GroupUsers
                .Where(x => x.UserId == userId)
                .Select(x => x.Group);
        }
    }
}
