using OnlineStore.Application.DTOs;
using OnlineStore.Application.Common;

namespace OnlineStore.Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderResponse> CheckoutAsync(Guid userId, CheckoutRequest request);
    Task<PagedResult<OrderResponse>> GetMyOrdersAsync(Guid userId, int page, int size);
    Task<PagedResult<OrderResponse>> GetAllOrdersAsync(OrdersAdminQuery query);
}
