using OnlineStore.Domain.Common;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities;

public class Order : BaseEntity
{   //يمثل طلب العميل
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string ShippingAddress { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    
    // Navigation properties
    public virtual AppUser User { get; set; } = default!;
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
