using eshop.product.service.domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eshop.product.service.application.Events;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger, ISendEndpointProvider sendEndpointProvider)
    {
        _logger = logger;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogTrace("Publishing ProductCreatedEvent {event}", notification);

        ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:product.added.to.cart"));
        await sendEndpoint.Send(notification, cancellationToken);
    }
}