namespace eshop.product.service.application.Dtos;

/// <summary>
/// The request DTO for modifying the inventory of a <see cref="Product"/>.
/// </summary>
public class ModifyProductInventoryRequestDto
{
    public string ProductId { get; set; } = string.Empty;

    public int NewInventory { get; set; }
}