using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Domain.Entities;

namespace AnnouncementApi.Application.Mappers
{
    public static class AnnouncementMappers
    {
        public static AnnouncementDto AnnouncementToAnnouncementDto(this Announcement announcement)
        {
            return new AnnouncementDto
            {
                Id = announcement.Id,
                Title = announcement.Title ?? string.Empty,
                Content = announcement.Content ?? string.Empty,
                LastUpdated = announcement.LastUpdated,
                UserId = announcement.UserId,
                UserName = announcement.User?.UserName ?? string.Empty
            };
        }
    }
}
