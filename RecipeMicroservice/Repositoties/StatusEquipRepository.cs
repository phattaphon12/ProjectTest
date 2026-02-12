using MySql.Data.MySqlClient;
using RecipeMicroservice.Data;
using RecipeMicroservice.Models;
using System.Data;

namespace RecipeMicroservice.Repositoties
{
    public class StatusEquipRepository
    {
        private readonly MariaDbContext _context;

        public StatusEquipRepository(MariaDbContext context)
        {
            _context = context;
        }

        public async Task<List<EquipStatus>> GetAllStatusEquipAsync()
        {
            var statusEquipList = new List<EquipStatus>();
            var sql = @"SELECT 
                    s.equip_id,
                    e.brand,
                    e.model,
                    s.recipe_id,
                    r.lot_id,
                    r.wafer_size,
                    r.cutting_dept,
                    r.line_cut,
                    s.stage,
                    s.downloaded_by,
                    s.downloaded_date
                FROM status s
                INNER JOIN equipment e ON s.equip_id = e.equip_id
                INNER JOIN recipe r ON s.recipe_id = r.recipe_id
                WHERE r.flag IS true;";
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var statusEquip = new EquipStatus
                            {
                                equip_id = reader.GetInt32("equip_id"),
                                brand = reader.GetString("brand"),
                                model = reader.GetString("model"),
                                recipe_id = reader.GetInt32("recipe_id"),
                                lot_id = reader.GetString("lot_id"),
                                wafer_size = reader.GetInt32("wafer_size"),
                                cutting_dept = reader.GetInt32("cutting_dept"),
                                line_cut = reader.GetInt32("line_cut"),
                                stage = reader.IsDBNull("stage") ? string.Empty : reader.GetString("stage"),
                                downloaded_by = reader.IsDBNull("downloaded_by") ? string.Empty : reader.GetString("downloaded_by"),
                                downloaded_date = reader.GetDateTime("downloaded_date")
                            };
                            statusEquipList.Add(statusEquip);
                        }
                    }
                }
            }
            return statusEquipList;
        }

        public async Task<EquipStatus?> GetStatusByEquipAsync(int EquipID)
        {
            EquipStatus? statusEquip = null;
            var sql = @"SELECT 
                    s.equip_id,
                    e.brand,
                    e.model,
                    s.recipe_id,
                    r.lot_id,
                    r.wafer_size,
                    r.cutting_dept,
                    r.line_cut,
                    s.stage,
                    s.downloaded_by,
                    s.downloaded_date
                FROM status s
                INNER JOIN equipment e ON s.equip_id = e.equip_id
                INNER JOIN recipe r ON s.recipe_id = r.recipe_id
                WHERE e.equip_id = @EquipID
                AND r.flag IS true;";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EquipID", EquipID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            statusEquip = new EquipStatus
                            {
                                equip_id = reader.GetInt32("equip_id"),
                                brand = reader.GetString("brand"),
                                model = reader.GetString("model"),
                                recipe_id = reader.GetInt32("recipe_id"),
                                lot_id = reader.GetString("lot_id"),
                                wafer_size = reader.GetInt32("wafer_size"),
                                cutting_dept = reader.GetInt32("cutting_dept"),
                                line_cut = reader.GetInt32("line_cut"),
                                stage = reader.IsDBNull("stage") ? string.Empty : reader.GetString("stage"),
                                downloaded_by = reader.IsDBNull("downloaded_by") ? string.Empty : reader.GetString("downloaded_by"),
                                downloaded_date = reader.GetDateTime("downloaded_date")
                            };
                        }
                    }
                }
            }
            return statusEquip;
        }

        public async Task<int> UpdateStatusEquipAsync(FormUpdateStatusEquip req)
        {
            var sql = @"INSERT INTO status (equip_id, recipe_id, stage, downloaded_by)
                        VALUES (@EquipIDInput, @RecipeIDInput, @StageInput, @DownloadedByInput)
                        ON DUPLICATE KEY UPDATE
                            recipe_id = VALUES(recipe_id),
                            stage = VALUES(stage),
                            downloaded_by = VALUES(downloaded_by)";

            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EquipIDInput", req.EquipID);
                    command.Parameters.AddWithValue("@RecipeIDInput", req.RecipeID);
                    command.Parameters.AddWithValue("@StageInput", req.Stage);
                    command.Parameters.AddWithValue("@DownloadedByInput", req.Downloaded_by);
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
        }
    }
}
