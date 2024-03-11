using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace PriskollenServer.Library.Services;
public class DbDapperContext : IDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DbDapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("default") ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
}