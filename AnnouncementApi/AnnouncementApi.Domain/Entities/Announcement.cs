using System.ComponentModel.DataAnnotations.Schema;

namespace AnnouncementApi.Domain.Entities
{
    [Table("Announcements")]
    public class Announcement
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }

        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
