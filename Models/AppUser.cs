using Microsoft.AspNetCore.Identity;

namespace Homework1.Models
{
    public class AppUser
    {
        public Guid User_id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "";

    }
}
