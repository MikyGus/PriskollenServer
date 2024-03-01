using Dapper;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace PriskollenServer.Library.Services;
public class DataAccess : IDataAccess
{
    private readonly IConfiguration _config;
    private readonly ILogger<DataAccess> _logger;
    private readonly string _connectionString;

    public DataAccess(IConfiguration config, ILogger<DataAccess> logger)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
        _logger = logger;
    }

    public async Task<ErrorOr<T>> LoadSingleDataAsync<T>(string sql, object parameters)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        T result = await connection.QuerySingleAsync<T>(sql, parameters);
        _logger.LogDebug("Succeeded with call to database with parameters {Parameters} having value: {RetreivedRecord} using sql {sql}",
            parameters, result, sql);
        return result;
    }

    public async Task<ErrorOr<List<T>>> LoadMultipleDataAsync<T>(
        string sqlString,
        object parameters)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        IEnumerable<T> results = await connection.QueryAsync<T>(sqlString, parameters);
        _logger.LogDebug("Succeeded with call to database with parameters {Parameters} having value: {RetreivedRecord} using sql {sql}",
            parameters, results, sqlString);
        return results.ToList();
    }
}