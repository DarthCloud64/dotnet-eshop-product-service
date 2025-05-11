namespace eshop.product.service.domain.Events;

/// <summary>
/// Event that is triggered when a product is removed from a cart.
/// </summary>
public class ProductRemovedFromCartEvent : IEvent
{
    /// <summary>
    /// The unique event id.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The product id.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;
}