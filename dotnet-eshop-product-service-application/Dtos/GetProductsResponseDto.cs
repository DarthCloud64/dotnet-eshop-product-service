namespace eshop.product.service.application.Dtos;

/// <summary>
/// The response DTO containing a collection of <see cref="Product"/>.
/// </summary>
public class GetProductsResponseDto
{
    public IList<GetProductResponseDto> Products { get; } = new List<GetProductResponseDto>();
}