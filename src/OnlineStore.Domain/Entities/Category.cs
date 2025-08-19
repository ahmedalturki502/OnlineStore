using OnlineStore.Domain.Common;

namespace OnlineStore.Domain.Entities;

public class Category : BaseEntity
{  //هذا يمثل فئة المنتج
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
