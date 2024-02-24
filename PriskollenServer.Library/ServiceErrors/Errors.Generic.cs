using ErrorOr;

namespace PriskollenServer.Library.ServiceErrors;
public static partial class Errors
{
    public static class Generic
    {
        public static Error NotFound(string displayName) => Error.NotFound(
            $"{displayName}.NotFound",
            $"{displayName} not found");
    }
}