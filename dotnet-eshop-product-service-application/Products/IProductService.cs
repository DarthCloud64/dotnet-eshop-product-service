using eshop.product.service.application.Dtos;

namespace eshop.product.service.application.Products;

public interface IProductService
{
    Task<CreateProductResponseDto> CreateProductAsync(CreateProductRequestDto createProductRequestDto, CancellationToken cancellationToken);
    Task<GetProductsResponseDto> GetAllProductsAsync(CancellationToken cancellationToken);
    Task<GetProductsResponseDto> GetProductByIdAsync(string productId, CancellationToken cancellationToken);
    Task ModifyProductInventory(ModifyProductInventoryRequestDto modifyProductInventoryRequestDto, CancellationToken cancellationToken);
    Task DecrementProductInventory(string productId, CancellationToken cancellationToken);
    Task IncrementProductInventory(string productId, CancellationToken cancellationToken);
}