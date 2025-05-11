using eshop.product.service.domain.Events;
using eshop.product.service.domain.Products;
using MongoDB.Driver;

namespace eshop.product.service.persistence.Uow;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    List<IEvent> Events { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
}