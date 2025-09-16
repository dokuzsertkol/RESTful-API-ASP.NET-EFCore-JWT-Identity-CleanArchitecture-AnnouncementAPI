using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Application.Mappers;

namespace AnnouncementApi.Infrastructure.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepo;

        public AnnouncementService(IAnnouncementRepository announcementRepo)
        {
            _announcementRepo = announcementRepo;
        }

        public async Task<AnnouncementDto?> GetAnnouncementAsync(int id)
        {
            var announcement = await _announcementRepo.GetAnnouncement(id);
            if (announcement == null) return null;

            return announcement.AnnouncementToAnnouncementDto();
        }

        public async Task<AnnouncementDto?> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto updateAnnouncementDto)
        {
            var announcement = await _announcementRepo.UpdateAnnouncement(id, updateAnnouncementDto);
            if (announcement == null) return null;

            return announcement.AnnouncementToAnnouncementDto();
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            var group = await _announcementRepo.DeleteAnnouncement(id);
            if (group == null) return false;
            return true;
        }
    }
}
