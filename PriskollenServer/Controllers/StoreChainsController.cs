using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.Services.StoreChains;

namespace PriskollenServer.Controllers;

public class StoreChainsController : ApiController
{
    private readonly IStoreChainService _storeChainService;

    public StoreChainsController(IStoreChainService storeChainService)
    {
        _storeChainService = storeChainService;
    }

    [HttpPost]
    public IActionResult CreateStoreChain(StoreChainRequest newStoreChain)
    {
        var storeChain = new StoreChain()
        {
            Id = Guid.NewGuid(),
            Name = newStoreChain.Name,
            Image = newStoreChain.Image,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
        };

        ErrorOr<Created> createStoreChainResult = _storeChainService.CreateStoreChain(storeChain);

        return createStoreChainResult.Match(
            created => CreatedAtGetStoreChain(storeChain),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetStoreChain(Guid id)
    {
        ErrorOr<StoreChain> getStoreChainResult = _storeChainService.GetStoreChain(id);
        return getStoreChainResult.Match(
            storeChain => Ok(MapStoreChainResponse(storeChain)),
            errors => Problem(errors));
    }

    [HttpGet()]
    public IActionResult GetStoreChain()
    {
        ErrorOr<IEnumerable<StoreChain>> getStoreChainsResult = _storeChainService.GetStoreChains();
        return getStoreChainsResult.Match(
            storeChains => Ok(MapStoreChainResponse(storeChains)),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateStoreChain(Guid id, StoreChainRequest updatedStoreChain)
    {
        StoreChain storeChain = new()
        {
            Id = id,
            Name = updatedStoreChain.Name,
            Image = updatedStoreChain.Image,
            Modified = DateTime.UtcNow,
        };
        ErrorOr<Updated> UpdateStoreChainResult = _storeChainService.UpdateStoreChain(storeChain);

        return UpdateStoreChainResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    private static StoreChainResponse MapStoreChainResponse(StoreChain storeChain)
        => new(storeChain.Id,
               storeChain.Name,
               storeChain.Image,
               storeChain.Created,
               storeChain.Modified);

    private static IEnumerable<StoreChainResponse> MapStoreChainResponse(IEnumerable<StoreChain> storeChains)
    {
        foreach (StoreChain storeChain in storeChains)
        {
            yield return MapStoreChainResponse(storeChain);
        }
    }

    private CreatedAtActionResult CreatedAtGetStoreChain(StoreChain storeChain)
        => CreatedAtAction(
            actionName: nameof(GetStoreChain),
            routeValues: new { id = storeChain.Id },
            value: MapStoreChainResponse(storeChain));
}