using ErrorOr;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Services.StoreChains;
public class StoreChainService : IStoreChainService
{
    private static readonly Dictionary<Guid, StoreChain> _storeChains = [];

    public ErrorOr<Created> CreateStoreChain(StoreChain storeChain)
    {
        _storeChains.Add(storeChain.Id, storeChain);
        return Result.Created;
    }

    public ErrorOr<StoreChain> GetStoreChain(Guid id)
    {
        if (_storeChains.TryGetValue(id, out StoreChain storeChain))
        {
            return storeChain;
        }
        return Errors.StoreChain.NotFound;
    }

    public ErrorOr<IEnumerable<StoreChain>> GetStoreChains() => _storeChains.Values;
    public ErrorOr<Updated> UpdateStoreChain(StoreChain storeChain)
    {
        storeChain.Created = _storeChains[storeChain.Id].Created;
        _storeChains[storeChain.Id] = storeChain;
        return Result.Updated;
    }
}