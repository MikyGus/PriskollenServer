﻿using ErrorOr;
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
    private readonly IGpsPositionValidator _gpsPositionValidator;

    public StoresController(
        IStoreValidator storeValidator,
        IStoreService storeService,
        IGpsPositionValidator gpsPositionValidator)
    {
        _storeValidator = storeValidator;
        _storeService = storeService;
        _gpsPositionValidator = gpsPositionValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStore([FromBody] StoreRequest newStore)
    {
        ErrorOr<StoreRequest> storeRequestValidated = _storeValidator.Validate(newStore);
        if (storeRequestValidated.IsError)
        {
            return Problem(storeRequestValidated.Errors);
        }

        ErrorOr<Store> createNewStoreResult = await _storeService.CreateStore(storeRequestValidated.Value);
        return createNewStoreResult.Match(
            store => CreatedAtGetStore(store),
            errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStore(int id)
    {
        ErrorOr<Store> getStoreResult = await _storeService.GetStore(id);
        return getStoreResult.Match(
            store => Ok(MapStoreResponse(store)),
            errors => Problem(errors));
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllStores(GpsRequest gpsRequest)
    {
        ErrorOr<GpsRequest> gpsPositionRequestValidated = _gpsPositionValidator.Validate(gpsRequest);
        if (gpsPositionRequestValidated.IsError)
        {
            return Problem(gpsPositionRequestValidated.Errors);
        }

        ErrorOr<List<Store>> getStoresResult;
        if (gpsPositionRequestValidated.Value.Latitude is null)
        {
            getStoresResult = await _storeService.GetAllStores();
            return getStoresResult.Match(
                store => Ok(MapStoreResponse(store)),
                errors => Problem(errors));
        }
        getStoresResult = await _storeService.GetAllStoresByDistance(gpsRequest.Latitude!.Value, gpsRequest.Longitude!.Value);
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
            store.Modified,
            store.Distance);

    private static IEnumerable<StoreResponse> MapStoreResponse(IEnumerable<Store> stores)
    {
        foreach (Store store in stores)
        {
            yield return MapStoreResponse(store);
        }
    }

    private CreatedAtActionResult CreatedAtGetStore(Store store)
        => CreatedAtAction(
            actionName: nameof(GetStore),
            routeValues: new { id = store.Id },
            value: MapStoreResponse(store));
}