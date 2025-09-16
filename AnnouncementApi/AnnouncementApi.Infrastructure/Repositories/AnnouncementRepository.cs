using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.Queries;
using AnnouncementApi.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using AnnouncementApi.Application.Interfaces;

namespace AnnouncementApi.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AppDbContext _context;

        public AnnouncementRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // group service
        public async Task DeleteAllFromGroupIdAsync(int groupId)
        {
            await _context.Announcements
               .Where(x => x.GroupId == groupId)
               .ExecuteDeleteAsync();
        }

        public async Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByGroupId(StringQueryObject query, int groupId)
        {
            var announcementDtos = _context.Announcements.Where(x => x.GroupId == groupId)
                .Select(x => new AnnouncementDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    LastUpdated = x.LastUpdated,
                    UserId = x.UserId,
                    UserName = x.User.UserName ?? string.Empty
                });

            if (!string.IsNullOrWhiteSpace(query.SearchFor))
            {
                announcementDtos = announcementDtos.Where(x => x.Title.ToLower()
                    .Contains(query.SearchFor.ToLower()) || x.Content.ToLower().Contains(query.SearchFor.ToLower()));
            }

            announcementDtos = announcementDtos.OrderByDescending(x => x.LastUpdated);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await announcementDtos.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Announcement?> AddAnnouncement(int groupId, AddAnnouncementDto addAnnouncementDto, string userId)
        {
            Announcement announcement = new()
            {
                Title = addAnnouncementDto.Title,
                Content = addAnnouncementDto.Content,
                GroupId = groupId,
                UserId = userId,
                LastUpdated = DateTime.Now
            };

            await _context.Announcements.AddAsync(announcement);
            await _context.SaveChangesAsync();

            var addedAnnouncement = await _context.Announcements
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == announcement.Id);
            if (addedAnnouncement == null) return null;

            return addedAnnouncement;
        }

        // announcement service
        public async Task<Announcement?> GetAnnouncement(int id)
        {
            return await _context.Announcements.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Announcement?> UpdateAnnouncement(int id, UpdateAnnouncementDto updateAnnouncementDto)
        {
            var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);

            if (announcement == null) return null;

            announcement.Title = updateAnnouncementDto.Title;
            announcement.Content = updateAnnouncementDto.Title;
            announcement.LastUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return announcement;
        }

        public async Task<Announcement?> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);

            if (announcement == null) return null;

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        // appuser service
        public async Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByUserId(StringQueryObject query, string userId)
        {
            var announcementDtos = _context.Announcements.Where(x => x.UserId == userId)
                .Select(x => new AnnouncementDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    LastUpdated = x.LastUpdated,
                    UserId = x.UserId,
                    UserName = x.User.UserName ?? string.Empty
                });

            if (!string.IsNullOrWhiteSpace(query.SearchFor))
            {
                announcementDtos = announcementDtos.Where(x => x.Title.ToLower()
                    .Contains(query.SearchFor.ToLower()) || x.Content.ToLower().Contains(query.SearchFor.ToLower()));
            }

            announcementDtos = announcementDtos.OrderByDescending(x => x.LastUpdated);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await announcementDtos.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }
    }
}
