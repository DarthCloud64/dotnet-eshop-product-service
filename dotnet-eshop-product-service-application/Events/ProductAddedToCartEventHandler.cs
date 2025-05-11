using eshop.product.service.application.Products;
using eshop.product.service.domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eshop.product.service.application.Events;

public class ProductAddedToCartEventHandler : IConsumer<ProductAddedToCartEvent>
{
    private readonly ILogger _logger;
    private readonly IProductService _productService;

    public ProductAddedToCartEventHandler(
        ILogger<ProductAddedToCartEventHandler> logger,
        IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductAddedToCartEvent> context)
    {
        await _productService.DecrementProductInventory(context.Message.ProductId, default);
    }
}