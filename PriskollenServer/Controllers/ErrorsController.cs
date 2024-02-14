using Microsoft.AspNetCore.Mvc;

namespace PriskollenServer.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }
}