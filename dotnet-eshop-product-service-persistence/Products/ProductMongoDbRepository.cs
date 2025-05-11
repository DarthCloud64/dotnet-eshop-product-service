using eshop.product.service.domain.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace eshop.product.service.persistence.Products;

public class ProductMongoDbRepository : IProductRepository
{
    private readonly ILogger _logger;
    private readonly IMongoCollection<Product> _productCollection;

    public ProductMongoDbRepository(
        IMongoClient mongoClient,
        IConfiguration configuration,
        ILogger<ProductMongoDbRepository> logger)
    {
        IConfigurationSection section = configuration.GetSection("DatabaseSettings");
        IMongoDatabase database = mongoClient.GetDatabase(section["MognoDb"]);
        _productCollection = database.GetCollection<Product>(section["MongoCollection"]);
        _logger = logger;
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await _productCollection.InsertOneAsync(product);

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            return (await _productCollection.FindAsync(filter)).FirstOrDefault();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error ocurred while creating a product");
            throw;
        }
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> ReadAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        return (await _productCollection.FindAsync(filter)).FirstOrDefault();
    }

    public async Task<List<Product>> ReadAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _productCollection.AsQueryable().ToListAsync();
    }

    public Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}