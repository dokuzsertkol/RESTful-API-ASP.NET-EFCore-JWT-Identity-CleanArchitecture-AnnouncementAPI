using AnnouncementApi.Domain.Entities;
using AnnouncementApi.Application.DTOs.Announcement;
using AnnouncementApi.Application.Queries;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IAnnouncementRepository
    {
        // group service
        Task DeleteAllFromGroupIdAsync(int groupId);
        Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByGroupId(StringQueryObject query, int groupId);
        Task<Announcement?> AddAnnouncement(int groupId, AddAnnouncementDto addAnnouncementDto, string userId);

        // announcement service
        Task<Announcement?> GetAnnouncement(int id);
        Task<Announcement?> UpdateAnnouncement(int id, UpdateAnnouncementDto updateAnnouncementDto);
        Task<Announcement?> DeleteAnnouncement(int id);

        // appuser service
        Task<IReadOnlyList<AnnouncementDto>> GetAnnouncementsByUserId(StringQueryObject query, string userId);
    }
}
