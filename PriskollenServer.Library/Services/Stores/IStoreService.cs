using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.Stores;
public interface IStoreService
{
    Task<ErrorOr<Store>> CreateStore(StoreRequest store);
    Task<ErrorOr<Store>> GetStore(int id);
    Task<ErrorOr<Store>> GetStoreWithDistance(int id, GpsRequest gpsRequest);
    Task<ErrorOr<List<Store>>> GetAllStores();
    Task<ErrorOr<List<Store>>> GetAllStoresByDistance(double latitude, double longitude);
    Task<ErrorOr<Updated>> UpdateStore(int id, StoreRequest store);
}