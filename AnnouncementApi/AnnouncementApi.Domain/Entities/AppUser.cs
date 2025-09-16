using Microsoft.AspNetCore.Identity;

namespace AnnouncementApi.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public List<GroupUser> GroupUsers { get; set; } = new();
        public List<Announcement> Announcements { get; set; } = new();
    }
}
