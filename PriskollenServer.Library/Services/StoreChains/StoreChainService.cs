using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;

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
        string sqlString = @"INSERT INTO storechains (name, image) 
	        VALUES (@Name, @Image)
	        RETURNING id, name, image, created, modified;";
        var parameters = new { storeChain.Name, storeChain.Image };

        try
        {
            ErrorOr<StoreChain> result = await _dataAccess.LoadSingleDataAsync<StoreChain>(sqlString, parameters);
            return result;
        }
        catch (Exception ex)
        {
            const string logErrorMessageTemplate = "Failed to create a new StoreChain using parameters {Parameters}";
            _logger.LogError(ex, logErrorMessageTemplate, parameters);
            return Errors.StoreChain.InsertFailure;
        }
    }

    public async Task<ErrorOr<StoreChain>> GetStoreChain(int id)
    {
        string sqlString = @"Select id, name, image, created, modified 
	        from storechains 
	        where id = @Id
	        order by name;";
        var parameters = new { Id = id };

        try
        {
            ErrorOr<StoreChain> storeChain = await _dataAccess.LoadSingleDataAsync<StoreChain>(sqlString, parameters);
            return storeChain;
        }
        catch (Exception ex)
        {
            const string logErrorMessageTemplate = "Failed to retreive StoreChain by using parameters {Parameters}";
            _logger.LogError(ex, logErrorMessageTemplate, parameters);
            return Errors.StoreChain.NotFound;
        }
    }

    public async Task<ErrorOr<List<StoreChain>>> GetAllStoreChains()
    {
        string sqlString = @"Select id, name, image, created, modified 
            from storechains 
            order by name;";
        var parameters = new { };

        try
        {
            ErrorOr<List<StoreChain>> storeChains = await _dataAccess.LoadMultipleDataAsync<StoreChain>(sqlString, parameters);
            return storeChains;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive StoreChains from the database with parameters {Parameters} using sql {sql}",
                parameters, sqlString);
            return Errors.StoreChain.NotFound;
        }
    }

    public async Task<ErrorOr<Updated>> UpdateStoreChain(int id, StoreChainRequest storeChain)
    {
        string sqlString = @"UPDATE storechains
	        SET name = @Name, 
		        image = @Image, 
		        modified = CURRENT_TIMESTAMP()
	        WHERE id = @Id;SELECT ROW_COUNT();";
        var parameters = new { Id = id, storeChain.Name, storeChain.Image };

        try
        {
            ErrorOr<int> result = await _dataAccess.LoadSingleDataAsync<int>(sqlString, parameters);
            if (result.Value == 1)
            {
                _logger.LogInformation("Updated StoreChain with Id: {Id} to values: {StoreChain}", id, storeChain);
                return Result.Updated;
            }
            // TODO: Make use of transaction to roll back
            _logger.LogError("Updated a number of {Count} StoreChain with Id: {Id} to values: {StoreChain}", result.Value, id, storeChain);
            return Result.Updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update StoreChain with Id: {Id} to values: {StoreChain}", id, storeChain);
            return Errors.StoreChain.NotFound;
        }
    }
}