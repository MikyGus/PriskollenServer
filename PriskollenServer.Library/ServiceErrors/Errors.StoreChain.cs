using ErrorOr;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Library.ServiceErrors;

public static class Errors
{
    public static class StoreChain
    {
        public static Error NotFound => Error.NotFound(
            "StoreChain.NotFound",
            "Store-chain not found");

        public static Error InvalidName => Error.Validation(
            "StoreChain.InvalidName",
            $"Name of Store-chain is not valid. Must be at least {StoreChainValidator.MinNameLength} in length " +
                $"and maximum {StoreChainValidator.MaxNameLength} in length.");
    }
}