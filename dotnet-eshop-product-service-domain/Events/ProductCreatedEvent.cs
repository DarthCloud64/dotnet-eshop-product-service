namespace eshop.product.service.domain.Events;

/// <summary>
/// Event that is triggered when a <see cref="Product"/> is created.
/// </summary>
public class ProductCreatedEvent : IEvent
{
    /// <summary>
    /// The unique event id.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The product id.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// The product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The product price.
    /// </summary>
    public float Price { get; set; }
}