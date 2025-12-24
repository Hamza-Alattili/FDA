using Application.DTOs.Auth;
using Application.DTOs.Student;
using Application.Services.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthControllers : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthControllers(IAuthService authService)
        {
            _authService = authService;
            
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto input)
        {
            var response = await _authService.LoginAsync(input);
            if (response == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(response);
        }
        [HttpPost("Resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto input)
        {
            await _authService.ResetPassword(input);
            return Ok("Password has been reset successfully.");
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var accessToken = await _authService.RefreshToken(refreshToken);
            return Ok(accessToken);
        }
        
    }
}
