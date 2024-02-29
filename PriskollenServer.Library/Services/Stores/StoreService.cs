using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;
using PriskollenServer.Library.Services.StoreChains;

namespace PriskollenServer.Library.Services.Stores;
public class StoreService : IStoreService
{
    private readonly IDataAccess _dataAccess;
    private readonly IConfiguration _config;
    private readonly ILogger<StoreChainService> _logger;
    private readonly string _connectionString;

    public StoreService(IDataAccess dataAccess, IConfiguration config, ILogger<StoreChainService> logger)
    {
        _dataAccess = dataAccess;
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
        _logger = logger;
    }

    public async Task<ErrorOr<Store>> CreateStore(StoreRequest store)
    {
        string sqlString = @"INSERT INTO stores (name, image, coordinate, address, city, storechain_id) 
	        VALUES (@Name, @Image, Point(@Longitude, @Latitude), @address, @city, @storechain_id)
	        RETURNING id, name, image, 
		        ST_Y(coordinate) as latitude, ST_X(coordinate) as longitude,
		        address, city, storechain_id, created, modified;";
        const string logErrorMessageTemplate = "Failed to create a new Store using parameters {Parameters}";

        try
        {
            ErrorOr<Store> result = await _dataAccess.LoadSingleDataAsync<Store>(sqlString, store);
            return result;
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, logErrorMessageTemplate, store);
            return Errors.Store.InvalidStoreChainId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, logErrorMessageTemplate, store);
            return Errors.Store.NotFound;
        }
    }

    public async Task<ErrorOr<List<Store>>> GetAllStores()
    {
        string storedProcedure = "GetAllStores";
        var parameters = new { };
        ErrorOr<List<Store>> stores = await _dataAccess.LoadMultipleDataAsync<Store>(storedProcedure, parameters, nameof(Store));
        return stores;
    }

    public Task<ErrorOr<List<Store>>> GetAllStoresByDistance(double latitude, double longitude) => throw new NotImplementedException();
    public async Task<ErrorOr<Store>> GetStore(int id)
    {
        string sql = @"Select id, name, image, 
 		    ST_Y(coordinate) as latitude, ST_X(coordinate) as longitude,
		    address, city, storechain_id, created, modified 
	        from stores
	        where id = searchForId;";
        var parameters = new { SearchForId = id };
        try
        {
            ErrorOr<Store> result = await _dataAccess.LoadSingleDataAsync<Store>(sql, parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Store by using parameters {Parameters}", parameters);
            return Errors.Store.NotFound;
        }
    }

    public Task<ErrorOr<Updated>> UpdateStore(int id, StoreChain store) => throw new NotImplementedException();
}