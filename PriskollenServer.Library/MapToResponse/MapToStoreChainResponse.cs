using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.MapToResponse;
public class MapToStoreChainResponse : IMapToResponse<StoreChain, StoreChainResponse>
{
    public IEnumerable<StoreChainResponse> MapToResponse(IEnumerable<StoreChain> models)
    {
        foreach (StoreChain model in models)
        {
            yield return MapToResponse(model);
        }
    }

    public StoreChainResponse MapToResponse(StoreChain model)
        => new(model.Id,
            model.Name,
            model.Image,
            model.Created,
            model.Modified);
}