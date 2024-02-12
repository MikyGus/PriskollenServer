using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreChainsController : ControllerBase
{
    [HttpPost()]
    public IActionResult CreateStoreChain(StoreChainRequest newStoreChain)
    {
        var storeChain = new StoreChain() { Id = Guid.NewGuid(), Name = newStoreChain.Name, Image = newStoreChain.Image };
        // TODO: Save the new StoreChain to the database
        var response = new StoreChainResponse(storeChain.Id, storeChain.Name, storeChain.Image);
        return CreatedAtAction(
            actionName: nameof(GetStoreChain),
            routeValues: new { id = storeChain.Id },
            value: response);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetStoreChain(Guid id)
    {
        return Ok(id);
    }

    [HttpGet()]
    public IActionResult GetStoreChain()
    {
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateStoreChain(Guid id, StoreChainRequest updatedStoreChain)
    {
        return Ok(id);
    }
}