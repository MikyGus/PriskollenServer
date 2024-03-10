using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.MapToResponse;
public class MapToStoreResponse : IMapToResponse<Store, StoreResponse>
{
    private IMapToResponse<StoreChain, StoreChainResponse> _mapToChainResponse;
    public MapToStoreResponse(IMapToResponse<StoreChain, StoreChainResponse> mapToChainResponse)
    {
        _mapToChainResponse = mapToChainResponse;
    }
    public StoreResponse MapToResponse(Store model)
        => new(
        model.Id,
        model.Name,
        model.Image,
        model.Latitude,
        model.Longitude,
        model.Address,
        model.City,
        model.StoreChain is not null ? _mapToChainResponse.MapToResponse(model.StoreChain) : null,
        model.Created,
        model.Modified,
        model.Distance);

    public IEnumerable<StoreResponse> MapToResponse(IEnumerable<Store> models)
    {
        foreach (Store model in models)
        {
            yield return MapToResponse(model);
        }
    }
}