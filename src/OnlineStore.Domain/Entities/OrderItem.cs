using OnlineStore.Domain.Common;

namespace OnlineStore.Domain.Entities;

public class OrderItem : BaseEntity
{  //يمثل عنصر من ضمن الطلب
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    
    // Navigation properties
    public virtual Order Order { get; set; } = default!;
    public virtual Product Product { get; set; } = default!;
}
