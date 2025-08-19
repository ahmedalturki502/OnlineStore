using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Interfaces.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByUserIdAsync(Guid userId);
}
