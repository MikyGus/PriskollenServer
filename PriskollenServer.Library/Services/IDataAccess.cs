using ErrorOr;

namespace PriskollenServer.Library.Services;
public interface IDataAccess
{
    Task<ErrorOr<T>> LoadSingleDataAsync<T>(string storedProcedure, object parameters, string errorDisplayName);
}