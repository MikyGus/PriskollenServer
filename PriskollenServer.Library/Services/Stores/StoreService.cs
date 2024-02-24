using Dapper;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;
using PriskollenServer.Library.Services.StoreChains;
using System.Data;

namespace PriskollenServer.Library.Services.Stores;
public class StoreService : IStoreService
{
    private readonly IConfiguration _config;
    private readonly ILogger<StoreChainService> _logger;
    private readonly string _connectionString;

    public StoreService(IConfiguration config, ILogger<StoreChainService> logger)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
        _logger = logger;
    }

    public Task<ErrorOr<Store>> CreateStore(StoreRequest store) => throw new NotImplementedException();
    public async Task<ErrorOr<List<Store>>> GetAllStores()
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        string storedProcedure = "GetAllStores";
        var parameters = new { };

        try
        {
            IEnumerable<Store> stores = await connection.QueryAsync<Store>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Retreived list of stores");
            return stores.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive all Stores from the database");
            return Errors.Store.NotFound;
        }
    }

    public Task<ErrorOr<List<Store>>> GetAllStoresByDistance(double latitude, double longitude) => throw new NotImplementedException();
    public async Task<ErrorOr<Store>> GetStore(int id)
    {
        using IDbConnection connection = new MySqlConnection(_connectionString);
        string storedProcedure = "GetStoreById";
        var parameters = new { SearchForId = id };

        try
        {
            Store store = await connection.QuerySingleAsync<Store>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Retreived a single record of Store: {Store}", store);
            return store;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive a record from the database");
            return Errors.Store.NotFound;
        }
    }

    public Task<ErrorOr<Updated>> UpdateStore(int id, StoreChain store) => throw new NotImplementedException();
}