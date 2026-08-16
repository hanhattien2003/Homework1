using Homework1.BL.Interfaces;
using Homework1.DL.Interfaces;
using Homework1.DTOs;
using Homework1.Services;

namespace Homework1.BL
{
    public class AuthBL : IAuthBL
    {
        private readonly IUserDL _userDL;
        private readonly JwtService _jwtService;

        public AuthBL(
            IUserDL userDL,
            JwtService jwtService)
        {
            _userDL = userDL;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request)
        {
            var user = await _userDL.GetByUsernameAsync(
                request.Username
            );

            if (user == null)
            {
                return null;
            }

            var passwordCorrect =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash
                );

            if (!passwordCorrect)
            {
                return null;
            }

            var token = _jwtService.CreateToken(user);

            return new LoginResponse
            {
                AccessToken = token,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}
