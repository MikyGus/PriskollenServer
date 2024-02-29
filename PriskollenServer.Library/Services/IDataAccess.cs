using ErrorOr;

namespace PriskollenServer.Library.Services;
public interface IDataAccess
{
    Task<ErrorOr<List<T>>> LoadMultipleDataAsync<T>(string storedProcedure, object parameters, string displayName);
    Task<ErrorOr<T>> LoadSingleDataAsync<T>(string storedProcedure, object parameters);
}