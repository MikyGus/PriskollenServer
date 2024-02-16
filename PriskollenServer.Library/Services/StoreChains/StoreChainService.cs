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
        if (_storeChains.TryGetValue(id, out StoreChain? storeChain))
        {
            return storeChain is null
                ? Errors.StoreChain.NotFound
                : storeChain;
        }
        return Errors.StoreChain.NotFound;
    }

    public ErrorOr<IEnumerable<StoreChain>> GetStoreChains() => _storeChains.Values;
    public ErrorOr<Updated> UpdateStoreChain(StoreChain storeChain)
    {
        // INFO: This is a bit hacky, but should get better when we use a real database
        if (_storeChains.TryGetValue(storeChain.Id, out StoreChain? sc))
        {
            if (sc is null)
            {
                return Errors.StoreChain.NotFound;
            }
            ErrorOr<StoreChain> storeC = StoreChain.Create(storeChain.Name, storeChain.Image, storeChain.Id, sc.Created);
            _storeChains[storeChain.Id] = storeC.Value;
            return Result.Updated;
        }
        return Errors.StoreChain.NotFound;
    }
}