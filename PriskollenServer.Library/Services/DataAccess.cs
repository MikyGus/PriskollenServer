using Dapper;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PriskollenServer.Library.ServiceErrors;
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

    public async Task<ErrorOr<T>> LoadSingleDataAsync<T>(string sql, object parameters, string displayName)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        T result = await connection.QuerySingleAsync<T>(sql, parameters);
        _logger.LogDebug("{DisplayName} succeeded with parameters {Parameters} having value: {RetreivedRecord}",
            displayName, parameters, result);
        return result;
    }

    public async Task<ErrorOr<List<T>>> LoadMultipleDataAsync<T>(
        string storedProcedure,
        object parameters,
        string displayName)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);

        try
        {
            IEnumerable<T> results = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogDebug("{DisplayName} succeeded with {StoredProcedure} with parameters {Parameters} having value: {RetreivedRecord}",
                displayName, storedProcedure, parameters, results);
            return results.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{DisplayName} failed to call the database with {StoredProcedure} using parameters {Parameters}",
                displayName, storedProcedure, parameters);
            return Errors.Generic.NotFound(displayName);
        }
    }
}