using OnlineStore.Domain.Common;

namespace OnlineStore.Domain.Entities;

public class Cart : BaseEntity
{   //هذا يمثل العربة اللي بتحط فيها منتجاتك
    public Guid UserId { get; set; }
    
    // Navigation properties
    public virtual AppUser User { get; set; } = default!;
    public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
