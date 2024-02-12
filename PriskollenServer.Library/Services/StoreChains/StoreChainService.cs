using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainService : IStoreChainService
{
    private static readonly Dictionary<Guid, StoreChain> _storeChains = [];

    public void CreateStoreChain(StoreChain storeChain) => _storeChains.Add(storeChain.Id, storeChain);

    public StoreChain GetStoreChain(Guid id) => _storeChains[id];
}