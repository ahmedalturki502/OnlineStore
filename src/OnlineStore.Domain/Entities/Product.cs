using OnlineStore.Domain.Common;

namespace OnlineStore.Domain.Entities;

public class Product : BaseEntity
{//يمثل المنتج الواحد في المتجر
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    
    // Navigation properties
    public virtual Category Category { get; set; } = default!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
