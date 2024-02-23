using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.Stores;
public class StoreService : IStoreService
{
    public Task<ErrorOr<Store>> CreateStore(StoreRequest store) => throw new NotImplementedException();
    public Task<ErrorOr<List<Store>>> GetAllStore() => throw new NotImplementedException();
    public Task<ErrorOr<Store>> GetStore(int id) => throw new NotImplementedException();
    public Task<ErrorOr<Updated>> UpdateStore(int id, StoreChain store) => throw new NotImplementedException();
}