using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.Services.Products;
public interface IProductService
{
    Task<ErrorOr<Product>> CreateProduct(ProductRequest product);
    Task<ErrorOr<Product>> GetProductById(int id);
    Task<ErrorOr<Product>> GetProductByBarcode(string barcode);
    Task<ErrorOr<List<Product>>> GetAllProducts();
    Task<ErrorOr<Updated>> UpdateProduct(int id, ProductRequest product);
}