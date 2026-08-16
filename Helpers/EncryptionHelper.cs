using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Security.Cryptography;
using Aes = System.Security.Cryptography.Aes;
namespace Homework1.Helpers
{
    public class EncryptionHelper
    {
        private readonly IConfiguration _configuration;

        public EncryptionHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Encrypt(string plainText)
        {
            var key = Encoding.UTF8.GetBytes(
                _configuration["Encryption:Key"]!
            );

            using var aes = Aes.Create();

            aes.Key = key;

            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();

            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            var encryptedBytes = encryptor.TransformFinalBlock(
                plainBytes,
                0,
                plainBytes.Length
            );

            var result = new byte[
                aes.IV.Length + encryptedBytes.Length
            ];

            Buffer.BlockCopy(
                aes.IV,
                0,
                result,
                0,
                aes.IV.Length
            );

            Buffer.BlockCopy(
                encryptedBytes,
                0,
                result,
                aes.IV.Length,
                encryptedBytes.Length
            );

            return Convert.ToBase64String(result);
        }
        public string Decrypt(string cipherText)
        {
            var key = Encoding.UTF8.GetBytes(
                _configuration["Encryption:Key"]!
            );

            var fullBytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();

            aes.Key = key;

            var iv = new byte[aes.BlockSize / 8];

            var encryptedBytes = new byte[
                fullBytes.Length - iv.Length
            ];

            Buffer.BlockCopy(
                fullBytes,
                0,
                iv,
                0,
                iv.Length
            );

            Buffer.BlockCopy(
                fullBytes,
                iv.Length,
                encryptedBytes,
                0,
                encryptedBytes.Length
            );

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();

            var decryptedBytes = decryptor.TransformFinalBlock(
                encryptedBytes,
                0,
                encryptedBytes.Length
            );

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
