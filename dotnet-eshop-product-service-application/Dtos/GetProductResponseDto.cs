namespace eshop.product.service.application.Dtos;

/// <summary>
/// The response DTO containing a <see cref="Product"/>.
/// </summary>
public class GetProductResponseDto
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public float Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Inventory { get; set; }

    public short Stars { get; set; }

    public int NumberOfReviews { get; set; }
}