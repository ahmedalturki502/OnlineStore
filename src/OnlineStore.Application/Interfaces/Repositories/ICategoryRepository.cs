using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> NameExistsAsync(string name, Guid? excludeId = null);
}
