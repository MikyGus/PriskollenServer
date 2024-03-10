using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.MapToResponse;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.Services.StoreChains;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Controllers;

public class StoreChainsController : ApiController
{
    private readonly IStoreChainService _storeChainService;
    private IStoreChainValidator _validator;
    private IMapToResponse<StoreChain, StoreChainResponse> _map;

    public StoreChainsController(
        IStoreChainService storeChainService,
        IStoreChainValidator validator,
        IMapToResponse<StoreChain, StoreChainResponse> mapToResponse)
    {
        _storeChainService = storeChainService;
        _validator = validator;
        _map = mapToResponse;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStoreChain(StoreChainRequest newStoreChain)
    {
        ErrorOr<StoreChainRequest> storeChainRequestValidated = _validator.Validate(newStoreChain);
        if (storeChainRequestValidated.IsError)
        {
            return Problem(storeChainRequestValidated.Errors);
        }
        ErrorOr<StoreChain> createNewStoreChainResult = await _storeChainService.CreateStoreChain(storeChainRequestValidated.Value);
        return createNewStoreChainResult.Match(
            storechain => CreatedAtGetStoreChain(storechain),
            errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStoreChain(int id)
    {
        ErrorOr<StoreChain> getStoreChainResult = await _storeChainService.GetStoreChain(id);
        return getStoreChainResult.Match(
            storeChain => Ok(_map.MapToResponse(storeChain)),
            errors => Problem(errors));
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllStoreChains()
    {
        ErrorOr<List<StoreChain>> getStoreChainsResult = await _storeChainService.GetAllStoreChains();

        return getStoreChainsResult.Match(
            storeChains => Ok(_map.MapToResponse(storeChains)),
            errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStoreChain(int id, StoreChainRequest updatedStoreChain)
    {
        ErrorOr<StoreChainRequest> storeChainRequestValidated = _validator.Validate(updatedStoreChain);
        if (storeChainRequestValidated.IsError)
        {
            return Problem(storeChainRequestValidated.Errors);
        }
        ErrorOr<Updated> updateStoreChainResult = await _storeChainService.UpdateStoreChain(id, storeChainRequestValidated.Value);

        return updateStoreChainResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    private CreatedAtActionResult CreatedAtGetStoreChain(StoreChain storeChain)
        => CreatedAtAction(
            actionName: nameof(GetStoreChain),
            routeValues: new { id = storeChain.Id },
            value: _map.MapToResponse(storeChain));
}