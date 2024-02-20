using Dapper;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.DatabaseAccess;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;
using System.Data;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainServiceDatabase : IStoreChainService
{
    private readonly IDataAccess _dataAccess;
    private readonly IConfiguration _config;
    private readonly ILogger<StoreChainServiceDatabase> _logger;
    private readonly string _connectionString;

    public StoreChainServiceDatabase(IDataAccess dataAccess, IConfiguration config, ILogger<StoreChainServiceDatabase> logger)
    {
        _dataAccess = dataAccess;
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
        _logger = logger;
    }

    public async Task<ErrorOr<StoreChain>> CreateStoreChain(StoreChainRequest storeChain)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        string storedProcedure = "CreateStorechain";
        var values = new { storeChain.Name, storeChain.Image };

        try
        {
            StoreChain result = await connection.QuerySingleAsync<StoreChain>(storedProcedure, values, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Inserted a new StoreChain with values: {StoreChain}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert new StoreChain with values: {StoreChain}", storeChain);
            return Errors.StoreChain.InsertFailure;
        }
    }

    public async Task<ErrorOr<StoreChain>> GetStoreChain(int id)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        string storedProcedure = "GetStoreChain";
        var parameters = new { SearchForId = id };

        try
        {
            StoreChain storeChain = await connection.QuerySingleAsync<StoreChain>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Retreived a single record of StoreChain: {StoreChain}", storeChain);
            return storeChain;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive a record from the database");
            return Errors.StoreChain.NotFound;
        }
    }

    public async Task<ErrorOr<List<StoreChain>>> GetAllStoreChains()
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        string storedProcedure = "GetAllStorechains";
        var values = new { };

        try
        {
            IEnumerable<StoreChain> storeChains = await connection.QueryAsync<StoreChain>(storedProcedure, values, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Retreived a list of all StoreChains");
            return storeChains.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive all StoreChains from the database");
            return Errors.StoreChain.NotFound;
        }
    }

    public ErrorOr<Updated> UpdateStoreChain(StoreChain storeChain) => throw new NotImplementedException();
}