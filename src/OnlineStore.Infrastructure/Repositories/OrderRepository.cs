using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Common;
using OnlineStore.Application.Interfaces.Repositories;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class OrderRepository : EfRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<Order>> GetAdminOrdersAsync(int page, int size, string? email, DateOnly? from, DateOnly? to)
    {
        IQueryable<Order> query = _dbSet
            .Include(o => o.User)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product);

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(o => o.User.Email!.Contains(email));
        }

        if (from.HasValue)
        {
            var fromDateTime = from.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(o => o.OrderDate >= fromDateTime);
        }

        if (to.HasValue)
        {
            var toDateTime = to.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(o => o.OrderDate <= toDateTime);
        }

        var totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<Order>
        {
            Items = items,
            PageNumber = page,
            PageSize = size,
            TotalCount = totalCount
        };
    }
}
