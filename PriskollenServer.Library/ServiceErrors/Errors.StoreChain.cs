using ErrorOr;

namespace PriskollenServer.Library.ServiceErrors;

public static class Errors
{
    public static class StoreChain
    {
        public static Error NotFound => Error.NotFound(
            "StoreChain.NotFound",
            "Store chain not found");
    }
}