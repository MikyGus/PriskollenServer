using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Validators;
public class StoreValidator : IStoreValidator
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 30;
    public const int MinLatitude = -90;
    public const int MaxLatitude = 90;
    public const int MinLongitude = -180;
    public const int MaxLongitude = 180;

    public bool IsValid(StoreRequest store, out List<Error> errors)
    {
        errors = [];
        if (store.Name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Store.InvalidName);
        }

        if (store.Latitude is < MinLatitude or > MaxLatitude)
        {
            errors.Add(Errors.Store.InvalidLatitude);
        }

        if (store.Longitude is < MinLongitude or > MaxLongitude)
        {
            errors.Add(Errors.Store.InvalidLongitude);
        }

        return errors.Count == 0;
    }

    public ErrorOr<StoreRequest> Validate(StoreRequest store)
        => IsValid(store, out List<Error> errors) ? store : errors;
}