namespace eshop.product.service.application.Dtos;

/// <summary>
/// Request DTO for creating a <see cref="Product"/>.
/// </summary>
public class CreateProductRequestDto
{
    /// <summary>
    /// The name of the product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The price of the product
    /// </summary>
    public float Price { get; set; }

    /// <summary>
    /// The product's description
    /// </summary>
    public string Description { get; set; } = string.Empty;
}