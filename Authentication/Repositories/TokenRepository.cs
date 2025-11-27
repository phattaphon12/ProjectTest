using Authentication.Data;
using Authentication.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Authentication.Repositories
{
    public class TokenRepository
    {
        private readonly MariaDbContext _context;

        public TokenRepository(MariaDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveRefreshTokenAsync(int userId, string RefreshToken)
        {
            var sql = @"INSERT INTO `refreshToken` (user_id, refresh_token, expire_date, revoke_date, login_date)
                        VALUES (@UserId, @RefreshToken, DATE_ADD(NOW(), INTERVAL 1 DAY), null, NOW())
                        ON DUPLICATE KEY UPDATE 
                            refresh_token = VALUES(refresh_token), 
                            expire_date = VALUES(expire_date),
                            revoke_date = VALUES(revoke_date),
                            login_date = VALUES(login_date);";
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@RefreshToken", RefreshToken);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }

        public async Task<int> RevokeRefreshTokenAsync(string UserId)
        {
            var sql = @"UPDATE `refreshToken`
                        SET revoke_date = NOW(),
                        refresh_token = ''
                        WHERE user_id = @UserId;";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", UserId);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }

        public async Task<RefreshTokenDb> GetDataByRefreshTokenAsync(string RefreshToken)
        {
            var result = new RefreshTokenDb();
            var sql = @"SELECT user_id,
                        refresh_token,
                         expire_date,
                         revoke_date,
                         login_date
                 FROM refreshToken
                 WHERE refresh_token = @RefreshToken";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RefreshToken", RefreshToken);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new RefreshTokenDb
                            {
                                user_id = reader.GetInt32("user_id"),
                                refresh_token = reader.GetString("refresh_token"),
                                expire_date = reader.IsDBNull("expire_date") ? null : reader.GetDateTime("expire_date"),
                                revoke_date = reader.IsDBNull("revoke_date") ? null : reader.GetDateTime("revoke_date"),
                                login_date = reader.IsDBNull("login_date") ? null : reader.GetDateTime("login_date"),
                            };
                        }
                    }
                }
                return result;
            }
        }
    }
}
