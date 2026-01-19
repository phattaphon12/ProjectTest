using MySql.Data.MySqlClient;

namespace RecipeMicroservice.Data
{
    public class MariaDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MariaDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MariaDbConnection")!;
        }

        public MySqlConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
