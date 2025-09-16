using AnnouncementApi.Application.DTOs.User;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Application.Mappers;
using AnnouncementApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementApi.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<NewUserDto> RegisterAsync(RegisterDto registerDto)
        {
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                var token = _tokenService.CreateToken(appUser);
                if (roleResult.Succeeded) return appUser.UserToNewUserDto(token);
                else
                {
                    var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
            }
            else
            {
                var errors = string.Join("; ", createdUser.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (appUser == null) throw new Exception("User not found.");

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);
            if (!result.Succeeded) throw new Exception("Wrong password.");

            var token = _tokenService.CreateToken(appUser);
            return appUser.UserToNewUserDto(token);
        }
    }
}
