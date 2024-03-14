using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.MapToResponse;
using PriskollenServer.Library.Models;
using PriskollenServer.Library.Services.Products;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Controllers;

public class ProductsController : ApiController
{
    private readonly IProductService _productService;
    private readonly IValidator<ProductRequest> _validator;
    private readonly IMapToResponse<Product, ProductResponse> _map;

    public ProductsController(IProductService productService, IValidator<ProductRequest> validator, IMapToResponse<Product, ProductResponse> map)
    {
        _productService = productService;
        _validator = validator;
        _map = map;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductRequest newProduct)
    {
        ErrorOr<ProductRequest> productRequestValidated = _validator.Validate(newProduct);
        if (productRequestValidated.IsError)
        {
            return Problem(productRequestValidated.Errors);
        }
        ErrorOr<Product> createNewProductResult = await _productService.CreateProduct(productRequestValidated.Value);
        return createNewProductResult.Match(
            product => CreatedAtGetProduct(product),
            errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        ErrorOr<Product> getProductByIdResult = await _productService.GetProductById(id);
        return getProductByIdResult.Match(
            product => Ok(_map.MapToResponse(product)),
            errors => Problem(errors));
    }

    [HttpGet("barcode/{barcode:int}")]
    public async Task<IActionResult> GetProductByBarcode(int barcode)
    {
        ErrorOr<Product> getProductByBarcode = await _productService.GetProductByBarcode(barcode.ToString());
        return getProductByBarcode.Match(
            product => Ok(_map.MapToResponse(product)),
            errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        ErrorOr<List<Product>> getAllProducts = await _productService.GetAllProducts();
        return getAllProducts.Match(
            products => Ok(_map.MapToResponse(products)),
            errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductRequest request)
    {
        ErrorOr<ProductRequest> productRequestValidated = _validator.Validate(request);
        if (productRequestValidated.IsError)
        {
            return Problem(productRequestValidated.Errors);
        }

        ErrorOr<Updated> result = await _productService.UpdateProduct(id, productRequestValidated.Value);
        return result.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    private CreatedAtActionResult CreatedAtGetProduct(Product product)
        => CreatedAtAction(
            actionName: nameof(GetProduct),
            routeValues: new { id = product.Id },
            value: _map.MapToResponse(product));
}