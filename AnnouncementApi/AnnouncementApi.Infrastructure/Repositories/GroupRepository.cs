using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AnnouncementApi.Application.Interfaces;

namespace AnnouncementApi.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfExists(int id)
        {
            return await _context.Groups.AnyAsync(s => s.Id == id);
        }

        public async Task<Group> CreateAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group?> UpdateAsync(int id, UpdateGroupDto updateGroupDto)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);

            if (group == null) return null;

            group.Name = updateGroupDto.Name;

            await _context.SaveChangesAsync();

            return group;
        }

        public async Task<Group?> DeleteAsync(int id)
        {
            var stockModel = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null) return null;

            _context.Groups.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Group?> GetGroup(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Group?> GetGroupDetails(int id)
        {
            return _context.Groups
            .AsNoTracking()
            .Include(g => g.Announcements)
                .ThenInclude(a => a.User)
            .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.User)
            .Where(g => g.Id == id);
        }
    }
}
