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
	        RETURNING id;";
        const string logErrorMessageTemplate = "Failed to create a new Store using parameters {Parameters}";

        try
        {
            ErrorOr<int> newStoreId = await _dataAccess.LoadSingleDataAsync<int>(sqlString, store);
            if (newStoreId.IsError == false)
            {
                ErrorOr<Store> newStore = await GetStore(newStoreId.Value);
                return newStore;
            }
            return newStoreId.Errors;
        }
        catch (MySqlException ex)
        {
            // TODO: Add if statement to check if it fails on StoreChainId being invalid
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
        string sqlString = @"Select id, name, image, 
 		    ST_Y(coordinate) as latitude, ST_X(coordinate) as longitude,
		    address, city, storechain_id, created, modified 
	        from stores
	        order by name;";
        var parameters = new { };
        try
        {
            ErrorOr<List<Store>> result = await _dataAccess.LoadMultipleDataAsync<Store>(sqlString, parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive Store from the database with parameters {Parameters} using sql {sql}",
                parameters, sqlString);
            return Errors.Store.NotFound;
        }
    }

    public async Task<ErrorOr<List<Store>>> GetAllStoresByDistance(double latitude, double longitude)
    {
        string sqlString = @"Select id, name, image, 
		        ST_X(coordinate) as longitude, ST_Y(coordinate) as latitude,
	            address, city, storechain_id, created, modified,
		        ST_DISTANCE_SPHERE(coordinate,Point(@longitude, @latitude))/1000 as distance
	        from stores
	        order by distance;";
        var parameters = new { latitude, longitude };
        try
        {
            ErrorOr<List<Store>> result = await _dataAccess.LoadMultipleDataAsync<Store>(sqlString, parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive Store from the database with parameters {Parameters} using sql {sql}",
                parameters, sqlString);
            return Errors.Store.NotFound;
        }
    }

    public async Task<ErrorOr<Store>> GetStore(int id)
    {
        string sql = @"Select s.id, s.name, s.image, 
           ST_Y(s.coordinate) as latitude, ST_X(s.coordinate) as longitude,
          s.address, s.city, s.storechain_id, s.created, s.modified,
                c.id, c.name, c.image, c.created, c.modified
         from stores s
            left join storechains c ON s.storechain_id = c.id
         where s.id = @searchForId;";

        var parameters = new { SearchForId = id };
        try
        {
            using IDbConnection connection = new MySqlConnection(_connectionString);
            IEnumerable<Store> store = await connection.QueryAsync<Store, StoreChain, Store>(sql, (store, storechain) =>
            {
                store.StoreChain = storechain;
                return store;
            }, param: parameters, splitOn: "Id");
            if (store is not null)
            {
                _logger.LogDebug("Successfully retreived store {Store} with parameters {Parameters}", store, parameters);
                return store.First();
            }
            _logger.LogWarning("Store not found with parameters {Parameters}", parameters);
            return Errors.Store.NotFound;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Store by using parameters {Parameters}", parameters);
            return Errors.Store.NotFound;
        }
    }

    public async Task<ErrorOr<Store>> GetStoreWithDistance(int id, GpsRequest gpsRequest)
    {
        string sql = @"Select s.id, s.name, s.image, 
           ST_Y(s.coordinate) as latitude, ST_X(s.coordinate) as longitude,
          s.address, s.city, s.storechain_id, s.created, s.modified,
          ST_DISTANCE_SPHERE(s.coordinate,Point(@longitude, @latitude))/1000 as distance,
                c.id, c.name, c.image, c.created, c.modified
         from stores s
            left join storechains c ON s.storechain_id = c.id
         where s.id = @searchForId;";

        var parameters = new { SearchForId = id, gpsRequest.Latitude, gpsRequest.Longitude };

        try
        {
            using IDbConnection connection = new MySqlConnection(_connectionString);
            IEnumerable<Store> store = await connection.QueryAsync<Store, StoreChain, Store>(sql, (store, storechain) =>
            {
                store.StoreChain = storechain;
                return store;
            }, param: parameters, splitOn: "Id");
            if (store is not null)
            {
                _logger.LogDebug("Successfully retreived store {Store} with parameters {Parameters}", store, parameters);
                return store.First();
            }
            _logger.LogWarning("Store not found with parameters {Parameters}", parameters);
            return Errors.Store.NotFound;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Store by using parameters {Parameters}", parameters);
            return Errors.Store.NotFound;
        }
    }

    public async Task<ErrorOr<Updated>> UpdateStore(int id, StoreRequest store)
    {
        string sqlString = @"UPDATE stores
	        SET name = @Name, 
		        image = @Image, 
                coordinate = Point(@Longitude, @Latitude),
                address = @Address,
                city = @City,
                storechain_id = @Storechain_id,
		        modified = CURRENT_TIMESTAMP()
	        WHERE id = @Id;SELECT ROW_COUNT();"
        ;
        var parameters = new
        {
            Id = id,
            store.Name,
            store.Image,
            store.Latitude,
            store.Longitude,
            store.Address,
            store.City,
            store.StoreChain_id
        };
        const string logErrorMessageTemplate = "Failed to update Store with Id {Id} using parameters {Parameters}";

        try
        {
            ErrorOr<int> result = await _dataAccess.LoadSingleDataAsync<int>(sqlString, parameters);
            if (result.Value == 1)
            {
                _logger.LogInformation("Updated Store with Id: {Id} to values: {Store}", id, store);
                return Result.Updated;
            }
            // TODO: Make use of transaction to roll back
            _logger.LogError("Updated a number of {Count} Stores with Id: {Id} to values: {Store}", result.Value, id, store);
            return Result.Updated;
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, logErrorMessageTemplate, id, store);
            return Errors.Store.InvalidStoreChainId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, logErrorMessageTemplate, id, store);
            return Errors.StoreChain.NotFound;
        }
    }
}