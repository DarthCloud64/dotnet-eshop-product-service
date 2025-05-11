using MediatR;

namespace eshop.product.service.domain.Events;

public interface IEvent : INotification
{
    string Id { get; set; }
}