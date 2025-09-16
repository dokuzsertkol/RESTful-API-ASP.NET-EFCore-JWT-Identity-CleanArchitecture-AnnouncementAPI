using System.ComponentModel.DataAnnotations;

namespace AnnouncementApi.Application.Queries
{
    public class GroupQueryObject
    {
        public string? SearchFor { get; set; } = null;

        public bool IsDescending { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive number.")]
        public int UserPageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be a positive number less than 100.")]
        public int UserPageSize { get; set; } = 20;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive number.")]
        public int AnnouncementPageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be a positive number less than 100.")]
        public int AnnouncementPageSize { get; set; } = 20;
    }
}
