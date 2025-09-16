using AnnouncementApi.Application.DTOs.Announcement;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementDto?> GetAnnouncementAsync(int id);
        Task<AnnouncementDto?> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto updateAnnouncementDto);
        Task<bool> DeleteAnnouncementAsync(int id);
    }
}
