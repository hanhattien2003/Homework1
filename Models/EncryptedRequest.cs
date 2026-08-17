namespace Homework1.Models
{
    public class EncryptedRequest
    {
        public string EncryptedKey { get; set; } = string.Empty;

        public string Iv { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;
    }
}
