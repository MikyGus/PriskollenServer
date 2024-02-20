using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.StoreChains;
public interface IStoreChainService
{
    Task<ErrorOr<StoreChain>> CreateStoreChain(StoreChainRequest storeChain);
    Task<ErrorOr<StoreChain>> GetStoreChain(int id);
    Task<ErrorOr<List<StoreChain>>> GetAllStoreChains();
    Task<ErrorOr<Updated>> UpdateStoreChain(int id, StoreChainRequest storeChain);
}