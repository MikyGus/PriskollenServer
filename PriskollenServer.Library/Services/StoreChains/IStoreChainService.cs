using ErrorOr;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public interface IStoreChainService
{
    void CreateStoreChain(StoreChain storeChain);
    ErrorOr<StoreChain> GetStoreChain(Guid id);
    IEnumerable<StoreChain> GetStoreChains();
    void UpdateStoreChain(StoreChain storeChain);
}