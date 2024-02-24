using Dapper;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PriskollenServer.Library.ServiceErrors;
using PriskollenServer.Library.Services.StoreChains;
using System.Data;

namespace PriskollenServer.Library.Services;
public class DataAccess : IDataAccess
{
    private readonly IConfiguration _config;
    private readonly ILogger<StoreChainService> _logger;
    private readonly string _connectionString;

    public DataAccess(IConfiguration config, ILogger<StoreChainService> logger)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
        _logger = logger;
    }

    public async Task<ErrorOr<T>> LoadSingleDataAsync<T>(
        string storedProcedure,
        object parameters,
        string errorDisplayName)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        try
        {
            T result = await connection.QuerySingleAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogDebug("Retreived a single record from {StoredProcedure} with parameters {Parameters} having value: {RetreivedRecord}", storedProcedure, parameters, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive a record from the database");
            return Errors.Generic.NotFound(errorDisplayName);
        }
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
            _logger.LogDebug("Retreived a list of record from {StoredProcedure} with parameters {Parameters} having values: {RetreivedRecords}", storedProcedure, parameters, results);
            return results.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive records from the database");
            return Errors.Generic.NotFound(displayName);
        }
    }
}