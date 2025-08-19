using OnlineStore.Domain.Entities;
using OnlineStore.Application.Common;

namespace OnlineStore.Application.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> NameExistsAsync(string name, Guid? excludeId = null);
    Task<PagedResult<Product>> GetInStockPagedAsync(int page, int size, Guid? categoryId, string? keyword);
}
