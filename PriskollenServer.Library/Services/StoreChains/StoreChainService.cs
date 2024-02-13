using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainService : IStoreChainService
{
    private static readonly Dictionary<Guid, StoreChain> _storeChains = [];

    public void CreateStoreChain(StoreChain storeChain) => _storeChains.Add(storeChain.Id, storeChain);

    public StoreChain GetStoreChain(Guid id) => _storeChains[id];
    public IEnumerable<StoreChain> GetStoreChains() => _storeChains.Values;
    public void UpdateStoreChain(StoreChain storeChain)
    {
        storeChain.Created = _storeChains[storeChain.Id].Created;
        _storeChains[storeChain.Id] = storeChain;
    }
}