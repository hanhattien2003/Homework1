using Homework1.Models;
using Dapper;
using Homework1.DL.Interfaces;
using Homework1.Models;
using Npgsql;
namespace Homework1.DL.Interfaces
{
    public interface IUserDL
    {
        Task<AppUser?> GetByUsernameAsync(string username);
    }
}
