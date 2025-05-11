namespace eshop.product.service.domain.Products;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);
    Task<Product> ReadAsync(string id, CancellationToken cancellationToken);
    Task<List<Product>> ReadAllAsync(CancellationToken cancellationToken);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}