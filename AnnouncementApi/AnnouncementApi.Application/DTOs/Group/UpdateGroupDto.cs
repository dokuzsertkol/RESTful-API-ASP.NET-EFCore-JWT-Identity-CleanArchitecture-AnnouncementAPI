using System.ComponentModel.DataAnnotations;

namespace AnnouncementApi.Application.DTOs.Group
{
    public class UpdateGroupDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Group name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "Group name cannot exceed 20 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
