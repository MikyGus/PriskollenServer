using ErrorOr;
using PriskollenServer.Library.Contracts;

namespace PriskollenServer.Library.Validators;
public interface IValidator
{
    bool IsValid(StoreChainRequest storeChain, out List<Error> errors);
    ErrorOr<StoreChainRequest> Validate(StoreChainRequest storeChain);
}