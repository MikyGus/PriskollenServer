using ErrorOr;
using Microsoft.Extensions.Configuration;
using PriskollenServer.Library.DatabaseAccess;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainServiceDatabase : IStoreChainService
{
    private readonly IDataAccess _dataAccess;
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public StoreChainServiceDatabase(IDataAccess dataAccess, IConfiguration config)
    {
        _dataAccess = dataAccess;
        _config = config;
        _connectionString = _config.GetConnectionString("default") ?? throw new ArgumentNullException();
    }

    public ErrorOr<Created> CreateStoreChain(StoreChain storeChain) => throw new NotImplementedException();
    public async Task<ErrorOr<StoreChain>> GetStoreChain(int id)
    {
        string sql = "select * from storechains where id=@id";
        var parameters = new { Id = id };
        List<StoreChain> storechains = await _dataAccess.LoadData<StoreChain, dynamic>(sql, parameters, _connectionString);

        return storechains.Count > 0 && storechains.FirstOrDefault() is StoreChain storeChain
            ? storeChain
            : Errors.StoreChain.NotFound;
    }

    public async Task<ErrorOr<List<StoreChain>>> GetStoreChains()
    {
        string sql = "select * from storechains";
        List<StoreChain> storechains = await _dataAccess.LoadData<StoreChain, dynamic>(sql, new { }, _connectionString);
        return storechains;
    }

    public ErrorOr<Updated> UpdateStoreChain(StoreChain storeChain) => throw new NotImplementedException();
}