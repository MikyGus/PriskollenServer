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
}