using Authentication.Data;
using Authentication.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Authentication.Repositories
{
    public class AuthRepository
    {
        private readonly MariaDbContext _context;

        public AuthRepository(MariaDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string Username)
        {
            var result = new User();
            var sql = @"SELECT user_id,
	                    username,
                        password,
                        create_date,
                        update_date
                FROM FactoryDB.user
                WHERE username = @Username";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", Username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new User
                            {
                                user_id = reader.GetInt32("user_id"),
                                username = reader.GetString("username"),
                                password = reader.GetString("password"),
                                create_date = reader.GetDateTime("create_date"),
                                update_date = reader.GetDateTime("update_date")
                            };
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> RegisterUserAsync(RegisterReq req)
        {
            var sql = @"INSERT INTO user (username, password)
                        values (@Username, @Password)";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", req.username);
                    command.Parameters.AddWithValue("@Password", req.password);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }

        public async Task<int> ChangePasswordAsync(string Username, string Password)
        {
            var sql = @"UPDATE user
                        SET password = @Password
                        WHERE username = @Username;";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Password", Password);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }
    }
}
