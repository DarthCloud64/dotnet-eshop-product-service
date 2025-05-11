using System.Net;
using eshop.product.service.application.Dtos;
using eshop.product.service.application.Products;
using Microsoft.AspNetCore.Mvc;

namespace eshop.product.service.webapi;

[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Gets a product by id.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>The product.</returns>
    [HttpGet("products/{productId}")]
    public async Task<IActionResult> GetProductById([FromRoute] string productId)
    {
        return Ok();
    }

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>All products.</returns>
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        return Ok();
    }

    /// <summary>
    /// Creates a product.
    /// </summary>
    /// <returns></returns>
    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto, CancellationToken cancellationToken)
    {
        CreateProductResponseDto createProductResponseDto = await _productService.CreateProductAsync(createProductRequestDto, cancellationToken);
        return StatusCode((int)HttpStatusCode.Created, createProductResponseDto);
    }

    /// <summary>
    /// Modifies the inventory for a given product.
    /// </summary>
    /// <returns></returns>
    [HttpPut("products/modifyProductInventory")]
    public async Task<IActionResult> ModifyProductInventory()
    {
        return NoContent();
    }
}