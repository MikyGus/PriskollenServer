using ErrorOr;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Library.ServiceErrors;
public static partial class Errors
{
    public static class Store
    {
        public static Error InvalidName => Error.Validation(
            "Store.InvalidName",
            $"Name of Store is not valid. Must be at least {StoreValidator.MinNameLength} " +
            $"in length and maximum {StoreValidator.MaxNameLength} in length.");

        public static Error InvalidLatitude => Error.Validation(
            "Store.InvalidLatitude",
            $"Latitude must be between {StoreValidator.MinLatitude} " +
            $"and {StoreValidator.MaxLatitude} inclusive to be considered valid.");

        public static Error InvalidLongitude => Error.Validation(
            "Store.InvalidLongitude",
            $"Longitude must be between {StoreValidator.MinLongitude} " +
            $"and {StoreValidator.MaxLongitude} inclusive to be considered valid.");

        public static Error NotFound => Error.NotFound(
            "Store.NotFound",
            "Store not found");
    }
}