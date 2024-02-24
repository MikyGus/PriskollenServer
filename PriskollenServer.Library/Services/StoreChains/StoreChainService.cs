using Dapper;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;
using System.Data;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainService : IStoreChainService
{
    private readonly IDataAccess _dataAccess;
    private readonly IConfiguration _config;
    private readonly ILogger<StoreChainService> _logger;
    private readonly string _connectionString;

    public StoreChainService(IConfiguration config, ILogger<StoreChainService> logger, IDataAccess dataAccess)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
        _logger = logger;
        _dataAccess = dataAccess;
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
        string storedProcedure = "GetStoreChain";
        var parameters = new { SearchForId = id };
        ErrorOr<StoreChain> storeChain = await _dataAccess.LoadSingleDataAsync<StoreChain>(storedProcedure, parameters, nameof(StoreChain));
        return storeChain;
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

    public async Task<ErrorOr<Updated>> UpdateStoreChain(int id, StoreChainRequest storeChain)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        string storedProcedure = "UpdateStoreChain";
        var parameters = new { SearchForId = id, storeChain.Name, storeChain.Image };

        try
        {
            int affectedRows = await connection.QuerySingleAsync<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            if (affectedRows == 1)
            {
                _logger.LogInformation("Updated StoreChain with Id: {Id} to values: {StoreChain}", id, storeChain);
            }
            else
            {
                // TODO: Make use of transaction to roll back
                _logger.LogError("Updated a number of {Count} StoreChain with Id: {Id} to values: {StoreChain}", affectedRows, id, storeChain);
            }
            return Result.Updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update StoreChain with Id: {Id} to values: {StoreChain}", id, storeChain);
            return Errors.StoreChain.NotFound;
        }
    }
}