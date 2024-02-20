using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Validators;
public class StoreChainValidator : IValidator
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 30;

    public ErrorOr<StoreChainRequest> Validate(StoreChainRequest storeChain)
        => IsValid(storeChain, out List<Error> errors) ? storeChain : errors;

    public bool IsValid(StoreChainRequest storeChain, out List<Error> errors)
    {
        errors = [];
        if (storeChain.Name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.StoreChain.InvalidName);
        }
        return errors.Count == 0;
    }
}
