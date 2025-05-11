using eshop.product.service.application.Products;
using eshop.product.service.domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eshop.product.service.application.Events;

public class ProductRemovedFromCartEventHandler : IConsumer<ProductRemovedFromCartEvent>
{
    private readonly ILogger _logger;
    private readonly IProductService _productService;

    public ProductRemovedFromCartEventHandler(
        ILogger<ProductRemovedFromCartEventHandler> logger,
        IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductRemovedFromCartEvent> context)
    {
        await _productService.IncrementProductInventory(context.Message.ProductId, default);
    }
}