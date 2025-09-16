using System.ComponentModel.DataAnnotations.Schema;

namespace AnnouncementApi.Domain.Entities
{
    [Table("Groups")]
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Announcement> Announcements { get; set; } = new();
        public List<GroupUser> GroupUsers { get; set; } = new();
    }
}
