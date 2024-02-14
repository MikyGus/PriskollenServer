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

        _storeChainService.CreateStoreChain(storeChain);

        var response = new StoreChainResponse(storeChain.Id, storeChain.Name, storeChain.Image, storeChain.Created, storeChain.Modified);
        return CreatedAtAction(
            actionName: nameof(GetStoreChain),
            routeValues: new { id = storeChain.Id },
            value: response);
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
        IEnumerable<StoreChain> storeChains = _storeChainService.GetStoreChains();
        return Ok(storeChains);
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
        _storeChainService.UpdateStoreChain(storeChain);

        return NoContent();
    }

    private static StoreChainResponse MapStoreChainResponse(StoreChain storeChain)
        => new(storeChain.Id,
               storeChain.Name,
               storeChain.Image,
               storeChain.Created,
               storeChain.Modified);
}