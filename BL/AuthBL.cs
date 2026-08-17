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
            Console.WriteLine("========== AUTH BL ==========");
            Console.WriteLine($"Username nhận được: {request.Username}");

            var user = await _userDL.GetByUsernameAsync(
                request.Username
            );

            if (user == null)
            {
                Console.WriteLine("❌ KHÔNG TÌM THẤY USER");
                return null;
            }

            Console.WriteLine("✅ ĐÃ TÌM THẤY USER");
            Console.WriteLine($"Username DB: {user.Username}");
            Console.WriteLine($"Role DB: {user.Role}");

            var passwordCorrect =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash
                );

            Console.WriteLine(
                $"Password đúng: {passwordCorrect}"
            );

            if (!passwordCorrect)
            {
                Console.WriteLine("❌ PASSWORD SAI");
                return null;
            }

            Console.WriteLine("✅ PASSWORD ĐÚNG");

            var token = _jwtService.CreateToken(user);

            Console.WriteLine("✅ TOKEN ĐÃ TẠO");

            return new LoginResponse
            {
                AccessToken = token,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}
