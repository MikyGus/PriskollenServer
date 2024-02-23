using ErrorOr;
namespace PriskollenServer.Library.Validators;
public interface IValidator<TRequest>
{
    bool IsValid(TRequest storeChain, out List<Error> errors);
    ErrorOr<TRequest> Validate(TRequest storeChain);
}