using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.DTOs.User;

namespace AnnouncementApi.Application.DTOs.Group
{
    public class GroupDetailDto : GroupDto
    {
        public IReadOnlyList<AnnouncementDto> Announcements { get; set; } = Array.Empty<AnnouncementDto>();

        public IReadOnlyList<AppUserDto> Users { get; set; } = Array.Empty<AppUserDto>();
    }
}
