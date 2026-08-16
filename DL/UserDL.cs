using Dapper;
using Homework1.DL.Interfaces;
using Homework1.Models;
using Npgsql;

namespace Homework1.DL
{
    public class UserDL : IUserDL
    {
        private readonly IConfiguration _configuration;

        public UserDL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            var sql = """
            SELECT
                user_id,
                username,
                password_hash AS PasswordHash,
                role
            FROM app_user
            WHERE username = @Username
        """;

            return await connection.QueryFirstOrDefaultAsync<AppUser>(
                sql,
                new
                {
                    Username = username
                }
            );
        }
    }
}