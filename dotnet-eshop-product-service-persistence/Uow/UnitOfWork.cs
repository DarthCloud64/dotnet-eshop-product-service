using eshop.product.service.domain.Events;
using eshop.product.service.domain.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eshop.product.service.persistence.Uow;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger _logger;
    private readonly IMongoClient _mongoClient;
    private readonly IMediator _mediator;
    private IClientSessionHandle? _clientSessionHandle;

    public IProductRepository ProductRepository { get; }
    public List<IEvent> Events { get; }

    public UnitOfWork(
        ILogger<UnitOfWork> logger,
        IMongoClient mongoClient,
        IMediator mediator,
        IProductRepository productRepository)
    {
        _logger = logger;
        _mongoClient = mongoClient;
        _mediator = mediator;
        ProductRepository = productRepository;
        Events = new List<IEvent>();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogTrace("Beginning DB transaction.");

        _clientSessionHandle = await _mongoClient.StartSessionAsync();
        _clientSessionHandle.StartTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_clientSessionHandle is null)
        {
            InvalidOperationException exception = new InvalidOperationException("DB session has not been initiated");
            _logger.LogError(exception, exception.Message);
            throw exception;
        }

        try
        {
            _logger.LogTrace("Publishing events");
            Events.ForEach(e => _mediator.Publish(e));

            _logger.LogTrace("Committing DB transaction.");
            await _clientSessionHandle.CommitTransactionAsync();
        }
        catch (Exception exception)
        {
            await _clientSessionHandle.AbortTransactionAsync();
            _logger.LogError(exception, "Error occurred when comitting DB transaction");
            throw;
        }
        finally
        {
            _clientSessionHandle = null;
        }
    }
}