using ErrorOr;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainService : IStoreChainService
{
    private static readonly Dictionary<Guid, StoreChain> _storeChains = [];

    public void CreateStoreChain(StoreChain storeChain) => _storeChains.Add(storeChain.Id, storeChain);

    public ErrorOr<StoreChain> GetStoreChain(Guid id)
    {
        if (_storeChains.TryGetValue(id, out StoreChain storeChain))
        {
            return storeChain;
        }
        return Errors.StoreChain.NotFound;
    }

    public IEnumerable<StoreChain> GetStoreChains() => _storeChains.Values;
    public void UpdateStoreChain(StoreChain storeChain)
    {
        storeChain.Created = _storeChains[storeChain.Id].Created;
        _storeChains[storeChain.Id] = storeChain;
    }
}