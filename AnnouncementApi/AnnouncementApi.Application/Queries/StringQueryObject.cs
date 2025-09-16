using System.ComponentModel.DataAnnotations;

namespace AnnouncementApi.Application.Queries
{
    public class StringQueryObject
    {
        public string? SearchFor { get; set; } = null;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive number.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be a positive number.")]
        public int PageSize { get; set; } = 20;
    }
}
