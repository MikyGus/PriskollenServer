using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Validators;
public class ProductValidator : IValidator<ProductRequest>
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 30;

    public bool IsValid(ProductRequest request, out List<Error> errors)
    {
        errors = [];

        if (request.Name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Product.InvalidName);
        }

        if (string.IsNullOrEmpty(request.Barcode))
        {
            errors.Add(Errors.Product.InvalidBarcode);
        }

        if (request.Brand.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Product.InvalidBrandName);
        }

        if (request.Volume < 0)
        {
            errors.Add(Errors.Product.InvalidVolume);
        }

        return errors.Count == 0;
    }

    public ErrorOr<ProductRequest> Validate(ProductRequest request)
        => IsValid(request, out List<Error> errors) ? request : errors;
}