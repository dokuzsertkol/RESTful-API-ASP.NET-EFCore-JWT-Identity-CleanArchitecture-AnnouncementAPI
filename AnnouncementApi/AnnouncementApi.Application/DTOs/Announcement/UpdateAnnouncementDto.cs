using System.ComponentModel.DataAnnotations;

namespace AnnouncementApi.Application.DTOs.Announcement
{
    public class UpdateAnnouncementDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "Title cannot exceed 20 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Content must be at least 3 characters long.")]
        [MaxLength(200, ErrorMessage = "Content cannot exceed 200 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}
