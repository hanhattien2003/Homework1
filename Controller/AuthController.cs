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

        public AuthController(IAuthBL authBL)
        {
            _authBL = authBL;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            var result = await _authBL.LoginAsync(request);

            if (result == null)
            {
                return Unauthorized(
                    "Sai username hoặc password"
                );
            }

            return Ok(result);
        }

    }
}
