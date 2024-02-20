using ErrorOr;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public interface IStoreChainService
{
    ErrorOr<Created> CreateStoreChain(StoreChain storeChain);
    Task<ErrorOr<StoreChain>> GetStoreChain(int id);
    Task<ErrorOr<List<StoreChain>>> GetStoreChains();
    ErrorOr<Updated> UpdateStoreChain(StoreChain storeChain);
}