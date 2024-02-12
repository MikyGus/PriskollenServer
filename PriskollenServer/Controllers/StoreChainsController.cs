using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;

namespace PriskollenServer.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreChainsController : ControllerBase
{
    [HttpPost()]
    public IActionResult CreateStoreChain(StoreChainRequest newStoreChain)
    {
        return Ok(newStoreChain);
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
