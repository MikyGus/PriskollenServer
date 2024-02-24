using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.Services.Stores;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Controllers;

public class StoresController : ApiController
{
    private readonly IStoreService _storeService;
    private readonly IStoreValidator _storeValidator;

    public StoresController(
        IStoreValidator storeValidator,
        IStoreService storeService)
    {
        _storeValidator = storeValidator;
        _storeService = storeService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllStores()
    {
        ErrorOr<List<Store>> getStoresResult = await _storeService.GetAllStores();

        return getStoresResult.Match(
            store => Ok(MapStoreResponse(store)),
            errors => Problem(errors));
    }

    private static StoreResponse MapStoreResponse(Store store)
        => new(
            store.Id,
            store.Name,
            store.Image,
            store.Latitude,
            store.Longitude,
            store.Address,
            store.City,
            store.Storechain_id,
            store.Created,
            store.Modified);
    private static IEnumerable<StoreResponse> MapStoreResponse(IEnumerable<Store> stores)
    {
        foreach (Store store in stores)
        {
            yield return MapStoreResponse(store);
        }
    }
}