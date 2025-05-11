using eshop.product.service.application.Events;
using eshop.product.service.domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace eshop.product.service.application.tests.Events;

public class ProductCreatedEventHandlerTests
{
    [Fact]
    public async Task HandleThrowsWhenOperationCancelled()
    {
        // Arrange
        ProductCreatedEventHandler productCreatedEventHandler = new ProductCreatedEventHandler(
            new Mock<ILogger<ProductCreatedEventHandler>>().Object,
            new Mock<ISendEndpointProvider>().Object
        );

        // Act
        Func<Task> result = async () => await productCreatedEventHandler.Handle(new ProductCreatedEvent(), new CancellationToken(true));

        // Assert
        result.ShouldThrow<OperationCanceledException>();
    }

    [Fact]
    public async Task HandleSuccessful()
    {
        // Arrange
        ProductCreatedEvent productCreatedEvent = new ProductCreatedEvent();
        Mock<ISendEndpoint> sendEndpointMock = new Mock<ISendEndpoint>();
        Mock<ISendEndpointProvider> sendEndpointProviderMock = new Mock<ISendEndpointProvider>();
        sendEndpointProviderMock.Setup(endpoint => endpoint.GetSendEndpoint(It.IsAny<Uri>()))
            .ReturnsAsync(sendEndpointMock.Object);
        ProductCreatedEventHandler productCreatedEventHandler = new ProductCreatedEventHandler(
            new Mock<ILogger<ProductCreatedEventHandler>>().Object,
            sendEndpointProviderMock.Object
        );

        // Act
        await productCreatedEventHandler.Handle(productCreatedEvent, default);

        // Assert
        sendEndpointMock.Verify(endpoint => endpoint.Send(productCreatedEvent, It.IsAny<CancellationToken>()), Times.Once());
    }
}