using Homework1.DTOs;
using Homework1.Models;
using Homework1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controller
{
  
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly JwtService _jwtService;

            public AuthController(JwtService jwtService)
            {
                _jwtService = jwtService;
            }
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (request.Username != "admin" || request.Password != "admin")
            {
                return Unauthorized("Sai tai khoan hoac mat khau");
            }
            var user = new AppUser
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "",
                Role = "Admin"
            };
            var token = _jwtService.CreateToken(user);
            var response = new LoginResponse
            {
                AccessToken = token,
                Username = user.Username,
                Role = user.Role
            };
            return Ok(response);
        }
    }
    
}
