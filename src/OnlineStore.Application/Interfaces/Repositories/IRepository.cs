using System.Linq.Expressions;
using OnlineStore.Application.Common;

namespace OnlineStore.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<PagedResult<T>> GetPagedAsync(int page, int size, Expression<Func<T, bool>>? filter = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? order = null, string includeProps = "");
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
