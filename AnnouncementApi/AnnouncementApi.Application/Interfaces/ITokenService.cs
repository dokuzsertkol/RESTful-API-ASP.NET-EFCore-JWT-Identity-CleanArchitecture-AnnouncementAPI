using AnnouncementApi.Domain.Entities;

namespace AnnouncementApi.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
