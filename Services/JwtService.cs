using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Homework1.Models;
using Microsoft.IdentityModel.Tokens;
namespace Homework1.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string CreateToken(AppUser user)
        {
            // ==========================================
            // 1. LẤY SIGNING KEY
            // ==========================================

            var signingKeyText =
                _configuration["Jwt:SigningKey"]
                ?? throw new Exception("Thiếu Jwt:SigningKey");

            var signingKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(signingKeyText)
                );


            // ==========================================
            // 2. TẠO CREDENTIALS ĐỂ KÝ
            // ==========================================

            var signingCredentials =
                new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256
                );


            // ==========================================
            // 3. LẤY ENCRYPTION KEY
            // ==========================================

            var encryptionKeyText =
                _configuration["Jwt:EncryptionKey"]
                ?? throw new Exception("Thiếu Jwt:EncryptionKey");

            var encryptionKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(encryptionKeyText)
                );


            // ==========================================
            // 4. TẠO CREDENTIALS ĐỂ MÃ HÓA
            // ==========================================

            var encryptingCredentials =
                new EncryptingCredentials(
                    encryptionKey,
                    SecurityAlgorithms.Aes256KW,
                    SecurityAlgorithms.Aes256CbcHmacSha512
                );


            // ==========================================
            // 5. TẠO CLAIMS
            // ==========================================

            var claims = new[]
            {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.User_id.ToString()
            ),

            new Claim(
                ClaimTypes.Name,
                user.Username
            ),

            new Claim(
                ClaimTypes.Role,
                user.Role
            )
        };


            // ==========================================
            // 6. TẠO TOKEN DESCRIPTOR
            // ==========================================

            var tokenDescriptor =
                new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),

                    Issuer =
                        _configuration["Jwt:Issuer"],

                    Audience =
                        _configuration["Jwt:Audience"],

                    Expires =
                        DateTime.UtcNow.AddHours(1),

                    SigningCredentials =
                        signingCredentials,

                    EncryptingCredentials =
                        encryptingCredentials
                };


            // ==========================================
            // 7. TẠO JWE
            // ==========================================

            var tokenHandler =
                new JwtSecurityTokenHandler();

            var token =
                tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
