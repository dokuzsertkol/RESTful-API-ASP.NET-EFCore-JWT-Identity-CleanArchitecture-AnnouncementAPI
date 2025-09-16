using System.ComponentModel.DataAnnotations.Schema;

namespace AnnouncementApi.Domain.Entities
{
    [Table("GroupUsers")]
    public class GroupUser
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public bool IsAdmin { get; set; }
    }
}