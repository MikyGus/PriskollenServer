using Dapper;
using ErrorOr;
using Microsoft.Extensions.Logging;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.ServiceErrors;
using System.Data;

namespace PriskollenServer.Library.Services.Products;
public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IDbContext _dbContext;

    public ProductService(ILogger<ProductService> logger, IDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<Product>> CreateProduct(ProductRequest newProduct)
    {
        const string sqlQuery = @"INSERT INTO products (barcode, name, brand, image, volume, volume_with_liquid, volume_unit)
                VALUES (@Barcode, @Name, @Brand, @Image, @Volume, @VolumeWithLiquid, @VolumeUnit)
                RETURNING id;";

        try
        {
            using IDbConnection connection = _dbContext.CreateConnection();
            int result = await connection.QuerySingleAsync<int>(sqlQuery, newProduct);
            Product product = new() { Id = result };
            _logger.LogDebug("Successfully created a new {Model} with the result {Result} using parameters {Parameters}", nameof(Product), result, product);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create a new {Model} using parameters {Parameters}", nameof(Product), newProduct);
            return Errors.Product.NotFound;
        }
    }

    public Task<ErrorOr<List<Product>>> GetAllProducts() => throw new NotImplementedException();
    public Task<ErrorOr<Product>> GetProductByBarcode(string barcode) => throw new NotImplementedException();
    public Task<ErrorOr<Product>> GetProductById(int id) => throw new NotImplementedException();
    public Task<ErrorOr<Updated>> UpdateProduct(int id, ProductRequest product) => throw new NotImplementedException();
}