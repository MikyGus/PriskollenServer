using Dapper;
using ErrorOr;
using Microsoft.Extensions.Logging;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;
using System.Data;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainService : IStoreChainService
{
    private readonly ILogger<StoreChainService> _logger;
    private readonly IDbContext _dbContext;

    public StoreChainService(ILogger<StoreChainService> logger, IDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<StoreChain>> CreateStoreChain(StoreChainRequest storeChain)
    {
        string sqlString = @"INSERT INTO storechains (name, image) 
	        VALUES (@Name, @Image)
	        RETURNING id, name, image, created, modified;";
        var parameters = new { storeChain.Name, storeChain.Image };

        try
        {
            using IDbConnection connection = _dbContext.CreateConnection();
            StoreChain result = await connection.QuerySingleAsync<StoreChain>(sqlString, parameters);
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
            using IDbConnection connection = _dbContext.CreateConnection();
            StoreChain result = await connection.QuerySingleAsync<StoreChain>(sqlString, parameters);
            return result;
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
            using IDbConnection connection = _dbContext.CreateConnection();
            IEnumerable<StoreChain> result = await connection.QueryAsync<StoreChain>(sqlString, parameters);
            return result.ToList();
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
            using IDbConnection connection = _dbContext.CreateConnection();
            int result = await connection.QuerySingleAsync<int>(sqlString, parameters);
            if (result == 1)
            {
                _logger.LogInformation("Updated StoreChain with Id: {Id} to values: {StoreChain}", id, storeChain);
                return Result.Updated;
            }
            // TODO: Make use of transaction to roll back
            _logger.LogError("Updated a number of {Count} StoreChain with Id: {Id} to values: {StoreChain}", result, id, storeChain);
            return Result.Updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update StoreChain with Id: {Id} to values: {StoreChain}", id, storeChain);
            return Errors.StoreChain.NotFound;
        }
    }
}