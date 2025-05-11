namespace eshop.product.service.application.Dtos;

/// <summary>
/// The response DTO when creating a <see cref="Product"/>.
/// </summary>
public class CreateProductResponseDto
{
    /// <summary>
    /// The id of the created product
    /// </summary>
    public string Id { get; set; } = string.Empty;
}