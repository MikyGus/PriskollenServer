using ErrorOr;

namespace PriskollenServer.Library.Services;
public interface IDataAccess
{
    Task<ErrorOr<List<T>>> LoadMultipleDataAsync<T>(string storedProcedure, object parameters);
    Task<ErrorOr<T>> LoadSingleDataAsync<T>(string storedProcedure, object parameters);
}