using ErrorOr;
namespace PriskollenServer.Library.Validators;
public interface IValidator<TRequest>
{
    bool IsValid(TRequest request, out List<Error> errors);
    ErrorOr<TRequest> Validate(TRequest request);
}