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
    /// <param name="cancellationToken"></param>
    /// <returns>The product.</returns>
    [HttpGet("products/{productId}")]
    public async Task<IActionResult> GetProductById([FromRoute] string productId, CancellationToken cancellationToken)
    {
        return Ok(await _productService.GetProductByIdAsync(productId, cancellationToken));
    }

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>All products.</returns>
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        return Ok(await _productService.GetAllProductsAsync(cancellationToken));
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
    /// <param name="modifyProductInventoryRequestDto"><see cref="ModifyProductInventoryRequestDto"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    [HttpPut("products/modifyProductInventory")]
    public async Task<IActionResult> ModifyProductInventory([FromBody] ModifyProductInventoryRequestDto modifyProductInventoryRequestDto, CancellationToken cancellationToken)
    {
        await _productService.ModifyProductInventory(modifyProductInventoryRequestDto, cancellationToken);
        return NoContent();
    }
}