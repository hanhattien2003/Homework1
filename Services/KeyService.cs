using System.Security.Cryptography;

namespace Homework1.Services
{
    public class KeyService
    {
        private readonly RSA _rsa;

        public KeyService()
        {
            _rsa = RSA.Create(2048);
        }

        public string GetPublicKey()
        {
            return ExportPublicKeyPem();
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            return _rsa.Decrypt(
                encryptedData,
                RSAEncryptionPadding.OaepSHA256
            );
        }

        private string ExportPublicKeyPem()
        {
            return _rsa.ExportSubjectPublicKeyInfoPem();
        }
    }
}
