using OnlineStore.Domain.Entities;
using OnlineStore.Application.Common;

namespace OnlineStore.Application.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<PagedResult<Order>> GetAdminOrdersAsync(int page, int size, string? email, DateOnly? from, DateOnly? to);
}
