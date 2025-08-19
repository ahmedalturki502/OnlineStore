using OnlineStore.Domain.Common;

namespace OnlineStore.Domain.Entities;

public class CartItem : BaseEntity
{    //هذا الكلاس يمثل منتج في سلة التسوق
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
    // Navigation properties
    public virtual Cart Cart { get; set; } = default!;
    public virtual Product Product { get; set; } = default!;
}
