﻿using Dapper;
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
            ErrorOr<Product> product = await GetProductById(result);
            if (product.IsError == false)
            {
                _logger.LogDebug("Successfully created a new {Model} with the result {Result} using parameters {Parameters}", nameof(Product), result, product);
            }
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create a new {Model} using parameters {Parameters}", nameof(Product), newProduct);
            return Errors.Product.NotFound;
        }
    }

    public async Task<ErrorOr<List<Product>>> GetAllProducts()
    {
        const string sqlQuery = @"SELECT id, barcode, name, brand, image, volume, volume_with_liquid, volume_unit, created, modified
                FROM products";
        try
        {
            using IDbConnection connection = _dbContext.CreateConnection();
            IEnumerable<Product> products = await connection.QueryAsync<Product>(sqlQuery);
            _logger.LogDebug("Successfully retreived a collection of {Model} with the result {Result}", nameof(Product), products);
            return products.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive {Model}", nameof(Product));
            return Errors.Product.NotFound;
        }
    }

    public async Task<ErrorOr<Product>> GetProductByBarcode(string barcode)
    {
        const string sqlQuery = @"SELECT id, barcode, name, brand, image, volume, volume_with_liquid, volume_unit, created, modified
                FROM products WHERE barcode=@Barcode;";
        var parameters = new { barcode };

        try
        {
            using IDbConnection connection = _dbContext.CreateConnection();
            Product product = await connection.QuerySingleAsync<Product>(sqlQuery, parameters);
            _logger.LogDebug("Successfully retreived a {Model} with the result {Result} using parameters {Parameters}", nameof(Product), product, product);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive {Model} using parameters {Parameters}", nameof(Product), parameters);
            return Errors.Product.NotFound;
        }
    }

    public async Task<ErrorOr<Product>> GetProductById(int id)
    {
        const string sqlQuery = @"SELECT id, barcode, name, brand, image, volume, volume_with_liquid, volume_unit, created, modified
                FROM products WHERE id=@Id;";
        var parameters = new { id };

        try
        {
            using IDbConnection connection = _dbContext.CreateConnection();
            Product product = await connection.QuerySingleAsync<Product>(sqlQuery, parameters);
            _logger.LogDebug("Successfully retreived a {Model} with the result {Result} using parameters {Parameters}", nameof(Product), product, product);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retreive {Model} using parameters {Parameters}", nameof(Product), parameters);
            return Errors.Product.NotFound;
        }
    }

    public async Task<ErrorOr<Updated>> UpdateProduct(int id, ProductRequest product)
    {
        const string sqlQuery = @"UPDATE products
	        SET barcode = @Barcode,
                name = @Name, 
		        brand = @Brand,
                image = @Image,
                volume = @Volume,
                volume_with_liquid = @VolumeWithLiquid,
                volume_unit = @VolumeUnit,
		        modified = CURRENT_TIMESTAMP()
	        WHERE id = @Id;SELECT ROW_COUNT();";
        var parameters = new
        {
            id,
            product.Barcode,
            product.Name,
            product.Brand,
            product.Image,
            product.Volume,
            product.VolumeWithLiquid,
            product.VolumeUnit,
        };

        try
        {
            using IDbConnection connection = _dbContext.CreateConnection();
            int result = await connection.QuerySingleAsync<int>(sqlQuery, parameters);
            if (result == 1)
            {
                _logger.LogInformation("Updated {Model} with Id: {Id} to values: {Store}", nameof(Product), id, product);
                return Result.Updated;
            }
            // TODO: Make use of transaction to roll back
            _logger.LogError("Updated a number of {Count} {Model} with Id: {Id} to values: {Store}", result, nameof(Product), id, product);
            return Result.Updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update {Model} using parameters {Parameters}", nameof(Product), parameters);
            return Errors.Product.NotFound;
        }
    }
}