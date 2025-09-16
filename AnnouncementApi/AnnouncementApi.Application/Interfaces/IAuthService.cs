using AnnouncementApi.Application.DTOs.User;

namespace AnnouncementApi.Application.Interfaces
{
    public interface IAuthService
    {
        Task<NewUserDto> RegisterAsync(RegisterDto registerDto);
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
    }
}
