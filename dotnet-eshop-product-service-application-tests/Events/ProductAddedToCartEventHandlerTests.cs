

using eshop.product.service.application.Events;
using eshop.product.service.application.Products;
using eshop.product.service.domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace eshop.product.service.application.tests.Events;

public class ProductAddedToCartEventHandlerTests
{
    [Fact]
    public async Task ConsumeSuccessful()
    {
        // Arrange
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        ProductAddedToCartEventHandler productAddedToCartEventHandler = new ProductAddedToCartEventHandler(
            new Mock<ILogger<ProductAddedToCartEventHandler>>().Object,
            productServiceMock.Object);
        Mock<ConsumeContext<ProductAddedToCartEvent>> consumeContext = new Mock<ConsumeContext<ProductAddedToCartEvent>>();
        consumeContext.Setup(ctx => ctx.Message)
            .Returns(new ProductAddedToCartEvent
            {
                ProductId = "123"
            });

        // Act
        await productAddedToCartEventHandler.Consume(consumeContext.Object);

        // Assert
        productServiceMock.Verify(service => service.DecrementProductInventory("123", It.IsAny<CancellationToken>()), Times.Once());
    }
}