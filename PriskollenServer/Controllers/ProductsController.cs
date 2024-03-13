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
        return Ok();
    }

    private CreatedAtActionResult CreatedAtGetProduct(Product product)
        => CreatedAtAction(
            actionName: nameof(GetProduct),
            routeValues: new { id = product.Id },
            value: _map.MapToResponse(product));
}