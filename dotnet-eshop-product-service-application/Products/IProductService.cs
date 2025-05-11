using eshop.product.service.application.Dtos;

namespace eshop.product.service.application.Products;

public interface IProductService
{
    Task<CreateProductResponseDto> CreateProductAsync(CreateProductRequestDto createProductRequestDto, CancellationToken cancellationToken);
}