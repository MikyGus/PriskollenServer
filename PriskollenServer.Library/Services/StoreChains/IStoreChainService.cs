using ErrorOr;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public interface IStoreChainService
{
    ErrorOr<Created> CreateStoreChain(StoreChain storeChain);
    ErrorOr<StoreChain> GetStoreChain(Guid id);
    ErrorOr<IEnumerable<StoreChain>> GetStoreChains();
    ErrorOr<Updated> UpdateStoreChain(StoreChain storeChain);
}