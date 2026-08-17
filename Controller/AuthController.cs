using Homework1.BL.Interfaces;
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
        private readonly IAuthBL _authBL;
        private readonly EncryptionService _encryptionService;
        public AuthController(
    IAuthBL authBL,
    EncryptionService encryptionService)
        {
            _authBL = authBL;
            _encryptionService = encryptionService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(EncryptedRequest request)
        {
            var loginRequest =
                _encryptionService.Decrypt<LoginRequest>(
                    request
                );

            var result =
                await _authBL.LoginAsync(
                    loginRequest
                );

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Tên đăng nhập hoặc mật khẩu không đúng."
                });
            }

            return Ok(result);
        }

    }
}
