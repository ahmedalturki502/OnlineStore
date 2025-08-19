using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Common;
using OnlineStore.Application.Interfaces.Repositories;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Repositories;

public class ProductRepository : EfRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<bool> NameExistsAsync(string name, Guid? excludeId = null)
    {
        var query = _dbSet.Where(p => p.Name == name);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<PagedResult<Product>> GetInStockPagedAsync(int page, int size, Guid? categoryId, string? keyword)
    {
        IQueryable<Product> query = _dbSet.Include(p => p.Category)
            .Where(p => p.StockQuantity > 0);

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(p => p.Name.Contains(keyword) || 
                                   (p.Description != null && p.Description.Contains(keyword)));
        }

        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(p => p.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<Product>
        {
            Items = items,
            PageNumber = page,
            PageSize = size,
            TotalCount = totalCount
        };
    }
}
