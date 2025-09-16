namespace AnnouncementApi.Application.DTOs.Announcement
{
    public class AnnouncementDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
    
        public string Content { get; set; } = string.Empty;
    
        public DateTime LastUpdated { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
