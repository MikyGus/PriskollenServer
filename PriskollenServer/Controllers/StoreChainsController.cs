using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;

namespace PriskollenServer.Controllers;

[ApiController]
public class StoreChainsController : ControllerBase
{
    [HttpPost("/storechains")]
    public IActionResult CreateStoreChain(StoreChainRequest newStoreChain)
    {
        return Ok(newStoreChain);
    }

    [HttpGet("/storechains/{id:guid}")]
    public IActionResult GetStoreChain(Guid id)
    {
        return Ok(id);
    }

    [HttpGet("/storechains/")]
    public IActionResult GetStoreChain()
    {
        return Ok();
    }

    [HttpPut("/storechains/{id:guid}")]
    public IActionResult UpdateStoreChain(Guid id, StoreChainRequest updatedStoreChain)
    {
        return Ok(id);
    }

}
