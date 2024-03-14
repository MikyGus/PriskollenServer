using ErrorOr;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Library.ServiceErrors;
public partial class Errors
{
    public static class Product
    {
        public static Error InvalidName => Error.Validation(
            "Product.InvalidName",
            $"Name of Product is not valid. Must be at least {ProductValidator.MinNameLength} " +
            $"in length and maximum {ProductValidator.MaxNameLength} in length.");

        public static Error InvalidBarcode => Error.Validation(
            "Product.InvalidBarcode",
            $"The barcode is not valid. Must have a value");

        public static Error InvalidBrandName => Error.Validation(
            "Product.InvalidName",
            $"Name of brandname is not valid. Must be at least {ProductValidator.MinNameLength} " +
            $"in length and maximum {ProductValidator.MaxNameLength} in length.");

        public static Error InvalidVolume => Error.Validation(
            "Product.InvalidVolume",
            "Volume cannot be a negative value");
        public static Error NotFound => Error.NotFound(
            "Product.NotFound",
            "Product not found");
    }
}