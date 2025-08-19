using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartResponse> GetAsync(Guid userId);
    Task<CartResponse> AddAsync(Guid userId, AddCartItemRequest request);
    Task<CartResponse> UpdateAsync(Guid userId, Guid productId, int quantity);
    Task RemoveAsync(Guid userId, Guid productId);
    Task ClearAsync(Guid userId);
}
