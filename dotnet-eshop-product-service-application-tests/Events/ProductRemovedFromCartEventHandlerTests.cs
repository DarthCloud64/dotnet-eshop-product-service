

using eshop.product.service.application.Events;
using eshop.product.service.application.Products;
using eshop.product.service.domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace eshop.product.service.application.tests.Events;

public class ProductRemovedFromCartEventHandlerTests
{
    [Fact]
    public async Task ConsumeSuccessful()
    {
        // Arrange
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        ProductRemovedFromCartEventHandler productRemovedFromCartEventHandler = new ProductRemovedFromCartEventHandler(
            new Mock<ILogger<ProductRemovedFromCartEventHandler>>().Object,
            productServiceMock.Object);
        Mock<ConsumeContext<ProductRemovedFromCartEvent>> consumeContext = new Mock<ConsumeContext<ProductRemovedFromCartEvent>>();
        consumeContext.Setup(ctx => ctx.Message)
            .Returns(new ProductRemovedFromCartEvent
            {
                ProductId = "123"
            });

        // Act
        await productRemovedFromCartEventHandler.Consume(consumeContext.Object);

        // Assert
        productServiceMock.Verify(service => service.IncrementProductInventory("123", It.IsAny<CancellationToken>()), Times.Once());
    }
}