using Homework1.DTOs;

namespace Homework1.BL.Interfaces
{
    public interface IAuthBL
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
