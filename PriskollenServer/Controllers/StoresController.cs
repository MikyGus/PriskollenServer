﻿using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.MapToResponse;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.Services.Stores;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Controllers;

public class StoresController : ApiController
{
    private readonly IStoreService _storeService;
    private readonly IStoreValidator _storeValidator;
    private readonly IGpsPositionValidator _gpsPositionValidator;
    private IMapToResponse<Store, StoreResponse> _map;

    public StoresController(
        IStoreValidator storeValidator,
        IStoreService storeService,
        IGpsPositionValidator gpsPositionValidator,
        IMapToResponse<Store, StoreResponse> mapToResponse)
    {
        _storeValidator = storeValidator;
        _storeService = storeService;
        _gpsPositionValidator = gpsPositionValidator;
        _map = mapToResponse;
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
    public async Task<IActionResult> GetStore(int id, GpsRequest gpsRequest)
    {
        ErrorOr<GpsRequest> gpsPositionRequestValidated = _gpsPositionValidator.Validate(gpsRequest);
        if (gpsPositionRequestValidated.IsError)
        {
            return Problem(gpsPositionRequestValidated.Errors);
        }

        ErrorOr<Store> getStoreResult = gpsPositionRequestValidated.Value.PositionIsProvided
            ? await _storeService.GetStoreWithDistance(id, gpsPositionRequestValidated.Value)
            : await _storeService.GetStore(id);

        return getStoreResult.Match(
            store => Ok(_map.MapToResponse(store)),
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

        ErrorOr<List<Store>> getStoresResult = gpsPositionRequestValidated.Value.PositionIsProvided
            ? await _storeService.GetAllStoresByDistance(gpsRequest.Latitude!.Value, gpsRequest.Longitude!.Value)
            : await _storeService.GetAllStores();

        return getStoresResult.Match(
            store => Ok(_map.MapToResponse(store)),
            errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStore(int id, StoreRequest storeRequest)
    {
        ErrorOr<StoreRequest> storeRequestValidated = _storeValidator.Validate(storeRequest);
        if (storeRequestValidated.IsError)
        {
            return Problem(storeRequestValidated.Errors);
        }

        ErrorOr<Updated> updateStoreResult = await _storeService.UpdateStore(id, storeRequestValidated.Value);

        return updateStoreResult.Match(
            update => NoContent(),
            errors => Problem(errors));
    }

    private CreatedAtActionResult CreatedAtGetStore(Store store)
        => CreatedAtAction(
            actionName: nameof(GetStore),
            routeValues: new { id = store.Id },
            value: _map.MapToResponse(store));
}