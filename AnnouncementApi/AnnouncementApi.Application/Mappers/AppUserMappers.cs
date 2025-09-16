using AnnouncementApi.Application.DTOs.User;
using AnnouncementApi.Domain.Entities;

namespace AnnouncementApi.Application.Mappers
{
    public static class AppUserMappers
    {
        public static AppUserDto UserToUserDto(this AppUser appUser)
        {
            return new AppUserDto
            {
                Id = appUser.Id,
                UserName = appUser.UserName ?? string.Empty
            };
        }

        public static NewUserDto UserToNewUserDto(this AppUser appUser, string token)
        {
            return new NewUserDto
            {
                Id = appUser.Id,
                UserName = appUser.UserName ?? string.Empty,
                Email = appUser.Email ?? string.Empty,
                Token = token
            };
        }
    }
}
