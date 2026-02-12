

using MySql.Data.MySqlClient;
using RecipeMicroservice.Data;
using RecipeMicroservice.Models;
using System.Data;

namespace RecipeMicroservice.Repositoties
{
    public class RecipeRepository
    {
        private readonly MariaDbContext _context;
        public RecipeRepository(MariaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var results = new List<Recipe>();

            var sql = @"SELECT recipe_id, lot_id, wafer_size, cutting_dept, line_cut, created_by, created_date, updated_by, update_date, flag
                        FROM recipe
                        WHERE flag IS true";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var recipe = new Recipe
                            {
                                recipe_id = reader.GetInt32("recipe_id"),
                                lot_id = reader.GetString("lot_id"),
                                wafer_size = reader.GetInt32("wafer_size"),
                                cutting_dept = reader.GetInt32("cutting_dept"),
                                line_cut = reader.GetInt32("line_cut"),
                                created_by = reader.GetString("created_by"),
                                created_date = reader.GetDateTime("created_date"),
                                updated_by = reader.GetString("updated_by"),
                                update_date = reader.GetDateTime("update_date"),
                                flag = reader.GetBoolean("flag")
                            };
                            results.Add(recipe);
                        }
                    }
                }
            }
            return results;
        }

        public async Task<List<Recipe>> GetRecipeByLotIdAsync(FormGetRecipeByLotID req)
        {
            //Recipe? recipe = null;
            var sql = @"SELECT recipe_id, lot_id, wafer_size, cutting_dept, line_cut, 
                               created_by, created_date, updated_by, update_date, flag
                        FROM recipe
                        WHERE lot_id LIKE CONCAT('%', @Lot_id, '%') AND flag IS true
                        ORDER BY created_date DESC";

            using (var connections = _context.CreateConnection())
            {
                await connections.OpenAsync();
                using (var command = new MySqlCommand(sql, connections))
                {
                    command.Parameters.AddWithValue("@Lot_id", req.lot_id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var results = new List<Recipe>();
                        while (await reader.ReadAsync())
                        {
                            var recipe = new Recipe
                            {
                                recipe_id = reader.GetInt32("recipe_id"),
                                lot_id = reader.GetString("lot_id"),
                                wafer_size = reader.GetInt32("wafer_size"),
                                cutting_dept = reader.GetInt32("cutting_dept"),
                                line_cut = reader.GetInt32("line_cut"),
                                created_by = reader.GetString("created_by"),
                                created_date = reader.GetDateTime("created_date"),
                                updated_by = reader.GetString("updated_by"),
                                update_date = reader.GetDateTime("update_date"),
                                flag = reader.GetBoolean("flag")
                            };
                            results.Add(recipe);
                        }
                        return results;
                    }
                }
            }
        }

        public async Task<int> InsertRecipeAsync(FormInsertRecipe req)
        {
            var sql = @"INSERT INTO recipe (lot_id, wafer_size, cutting_dept, line_cut, created_by, updated_by)
            VALUES (@Lot_id, @Wafer_size, @Cutting_dept, @Line_cut, @Created_by, @Updated_by)";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Lot_id", req.lot_id);
                    command.Parameters.AddWithValue("@Wafer_size", req.wafer_size);
                    command.Parameters.AddWithValue("@Cutting_dept", req.cutting_dept);
                    command.Parameters.AddWithValue("@Line_cut", req.line_cut);
                    command.Parameters.AddWithValue("@Created_by", req.created_by);
                    command.Parameters.AddWithValue("@Updated_by", req.updated_by);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }

        public async Task<int> UpdateRecipeAsync(FormUpdateRecipe req)
        {
            var sql = @"UPDATE recipe
                        SET wafer_size = @Wafer_size,
                            cutting_dept = @Cutting_dept,
                            line_cut = @Line_cut,
                            updated_by = @Updated_by
                        WHERE lot_id = @Lot_id";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Wafer_size", req.wafer_size);
                    command.Parameters.AddWithValue("@Cutting_dept", req.cutting_dept);
                    command.Parameters.AddWithValue("@Line_cut", req.line_cut);
                    command.Parameters.AddWithValue("@Updated_by", req.updated_by);
                    command.Parameters.AddWithValue("@Lot_id", req.lot_id);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }

        public async Task<int> DeleteRecipeAsync(UnflagRecipe req)
        {
            var sql = @"UPDATE recipe
                        SET flag = 0,
                            updated_by = @Updated_by
                        WHERE lot_id = @Lot_id";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Lot_id", req.lot_id);
                    command.Parameters.AddWithValue("@Updated_by", req.updated_by);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }

        
    }
}
