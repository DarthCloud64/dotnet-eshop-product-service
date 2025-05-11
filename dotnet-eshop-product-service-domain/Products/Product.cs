namespace eshop.product.service.domain.Products;

/// <summary>
/// Represents a product.
/// </summary>
public class Product
{
    /// <summary>
    /// The unique identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The product's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The price.
    /// </summary>
    public float Price { get; set; }

    /// <summary>
    /// The description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The inventory in the warehouse
    /// </summary>
    public int Inventory { get; set; }

    /// <summary>
    /// Number of stars in the review
    /// </summary>
    public short Stars { get; set; }

    /// <summary>
    /// Number of reviews
    /// </summary>
    public int NumberOfReviews { get; set; }
}