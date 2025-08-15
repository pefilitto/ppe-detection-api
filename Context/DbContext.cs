using System.Data;
using MySql.Data.MySqlClient;

namespace ppe_detection_api.Context;

public class DbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
    }
    
    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}