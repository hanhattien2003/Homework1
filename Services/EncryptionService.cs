using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Homework1.Models;

namespace Homework1.Services
{
    public class EncryptionService
    {
        private readonly KeyService _keyService;

        public EncryptionService(KeyService keyService)
        {
            _keyService = keyService;
        }

        public T Decrypt<T>(EncryptedRequest request)
        {
            // ====================================
            // 1. Decode Base64
            // ====================================

            var encryptedKey =
                Convert.FromBase64String(
                    request.EncryptedKey
                );

            var iv =
                Convert.FromBase64String(
                    request.Iv
                );

            var encryptedData =
                Convert.FromBase64String(
                    request.Data
                );


            // ====================================
            // 2. Giải mã AES KEY bằng RSA
            // ====================================

            var aesKey =
                _keyService.Decrypt(
                    encryptedKey
                );


            // ====================================
            // 3. Tách ciphertext và authentication tag
            // ====================================

            const int tagSize = 16;

            var cipherLength =
                encryptedData.Length - tagSize;

            var ciphertext =
                encryptedData[..cipherLength];

            var tag =
                encryptedData[cipherLength..];


            // ====================================
            // 4. Giải mã DATA bằng AES-GCM
            // ====================================

            var plaintext =
                new byte[cipherLength];

            using var aes =
                new AesGcm(aesKey, tagSize);

            aes.Decrypt(
                iv,
                ciphertext,
                tag,
                plaintext
            );


            // ====================================
            // 5. Chuyển byte → JSON
            // ====================================

            var json =
                Encoding.UTF8.GetString(
                    plaintext
                );


            // ====================================
            // 6. JSON → Object
            // ====================================

                        var result =
                JsonSerializer.Deserialize<T>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            if (result == null)
            {
                throw new Exception(
                    "Không thể giải mã request"
                );
            }

            return result;
        }
    }
}
