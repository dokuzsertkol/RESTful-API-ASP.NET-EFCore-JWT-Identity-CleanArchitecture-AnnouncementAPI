using AnnouncementApi.Application.DTOs.User;
using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Responses;

using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApi.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var userDto = await _authService.RegisterAsync(registerDto);
                return Ok(new ApiResponse<NewUserDto> { Data = userDto, Message = "Registration completed successfully." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Message = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var userDto = await _authService.LoginAsync(loginDto);
                return Ok(new ApiResponse<NewUserDto> { Data = userDto, Message = "You have logged in successfully." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Message = e.Message });
            }
        }
    }
}
