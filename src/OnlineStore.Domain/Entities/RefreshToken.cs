using OnlineStore.Domain.Common;

namespace OnlineStore.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    
    // Navigation properties
    public virtual AppUser User { get; set; } = default!;
}
