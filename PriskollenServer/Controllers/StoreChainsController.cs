﻿using ErrorOr;
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

    //[HttpPost]
    //public IActionResult CreateStoreChain(StoreChainRequest newStoreChain)
    //{
    //    ErrorOr<StoreChain> requestToStoreChainResult = StoreChain.CreateFrom(newStoreChain);
    //    if (requestToStoreChainResult.IsError)
    //    {
    //        return Problem(requestToStoreChainResult.Errors);
    //    }

    //    StoreChain storeChain = requestToStoreChainResult.Value;

    //    ErrorOr<Created> createStoreChainResult = _storeChainService.CreateStoreChain(storeChain);

    //    return createStoreChainResult.Match(
    //        created => CreatedAtGetStoreChain(storeChain),
    //        errors => Problem(errors));
    //}

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStoreChain(int id)
    {
        ErrorOr<StoreChain> getStoreChainResult = await _storeChainService.GetStoreChain(id);
        return getStoreChainResult.Match(
            storeChain => Ok(MapStoreChainResponse(storeChain)),
            errors => Problem(errors));
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllStoreChains()
    {
        ErrorOr<List<StoreChain>> getStoreChainsResult = await _storeChainService.GetStoreChains();

        return getStoreChainsResult.Match(
            storeChains => Ok(MapStoreChainResponse(storeChains)),
            errors => Problem(errors));
    }

    //[HttpPut("{id:guid}")]
    //public IActionResult UpdateStoreChain(Guid id, StoreChainRequest updatedStoreChain)
    //{
    //    ErrorOr<StoreChain> RequestToStoreChainRequest = StoreChain.CreateFrom(id, updatedStoreChain);
    //    if (RequestToStoreChainRequest.IsError)
    //    {
    //        return Problem(RequestToStoreChainRequest.Errors);
    //    }
    //    StoreChain storeChain = RequestToStoreChainRequest.Value;

    //    ErrorOr<Updated> UpdateStoreChainResult = _storeChainService.UpdateStoreChain(storeChain);

    //    return UpdateStoreChainResult.Match(
    //        updated => NoContent(),
    //        errors => Problem(errors));
    //}

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

    //    private CreatedAtActionResult CreatedAtGetStoreChain(StoreChain storeChain)
    //        => CreatedAtAction(
    //            actionName: nameof(GetStoreChain),
    //            routeValues: new { id = storeChain.Id },
    //            value: MapStoreChainResponse(storeChain));
}