using MySql.Data.MySqlClient;
using RecipeMicroservice.Data;
using RecipeMicroservice.Models;
using System.Data;

namespace RecipeMicroservice.Repositoties
{
    public class EquipRepository
    {
        private readonly MariaDbContext _context;
        public EquipRepository(MariaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Equip>> GetAllEquipAsync()
        {
            var sql = @"SELECT equip_id, brand, model, created_by, created_date, updated_by, updated_date
                        FROM equipment";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    var equips = new List<Equip>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var equip = new Equip
                            {
                                equip_id = reader.GetInt32("equip_id"),
                                brand = reader.GetString("brand"),
                                model = reader.GetString("model"),
                                created_by = reader.GetString("created_by"),
                                created_date = reader.GetDateTime("created_date"),
                                updated_by = reader.GetString("updated_by"),
                                updated_date = reader.GetDateTime("updated_date")
                            };
                            equips.Add(equip);
                        }
                    }
                    return equips;
                }
            }
        }

        public async Task<List<LogEquip>> GetAllLogAsync()
        {
            var sql = @"SELECT equip_id, recipe_id, detail, time_stamp, user_by
                        FROM log";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    var logEquips = new List<LogEquip>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var logEquip = new LogEquip
                            {
                                EquipID = reader.GetInt32("equip_id"),
                                RecipeID = reader.GetInt32("recipe_id"),
                                Detail = reader.GetString("detail"),
                                time_stamp = reader.GetDateTime("time_stamp"),
                                user_by = reader.GetString("user_by")
                            };
                            logEquips.Add(logEquip);
                        }
                    }
                    return logEquips;
                }
            }
        }

        public async Task<List<LogEquip>> GetLogByEquipAsync(int equipID)
        {
            var sql = @"SELECT equip_id, recipe_id, detail, time_stamp, user_by
                        FROM log
                        WHERE equip_id = @EquipID";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EquipID", equipID);
                    var logEquips = new List<LogEquip>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var logEquip = new LogEquip
                            {
                                EquipID = reader.GetInt32("equip_id"),
                                RecipeID = reader.GetInt32("recipe_id"),
                                Detail = reader.GetString("detail"),
                                time_stamp = reader.GetDateTime("time_stamp"),
                                user_by = reader.GetString("user_by")
                            };
                            logEquips.Add(logEquip);
                        }
                    }
                    return logEquips;
                }
            }
        }

        public async Task<int> InsertLogAsync(FormLogEquip req)
        {
            var sql = @"INSERT INTO log(equip_id, recipe_id, detail, user_by)
                        VALUE (@EquipID, @RecipeID, @Stage, @UserBy)";
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EquipID", req.EquipID);
                    command.Parameters.AddWithValue("@RecipeID", req.RecipeID);
                    command.Parameters.AddWithValue("@detail", req.Detail);
                    command.Parameters.AddWithValue("@UserBy", req.user_by);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }
    }
}
