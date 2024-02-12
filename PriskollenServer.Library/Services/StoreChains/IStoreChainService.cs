using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public interface IStoreChainService
{
    void CreateStoreChain(StoreChain storeChain);
    StoreChain GetStoreChain(Guid id);
}