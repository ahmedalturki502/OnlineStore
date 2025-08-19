using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Common;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Repositories;
using OnlineStore.Application.Interfaces.Services;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper,
        AppDbContext context)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<OrderResponse> CheckoutAsync(Guid userId, CheckoutRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            // Validate stock and calculate total
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cart.Items)
            {
                var product = await _context.Products
                    .Where(p => p.Id == cartItem.ProductId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    throw new InvalidOperationException($"Product {cartItem.ProductId} not found.");
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");
                }

                var subtotal = product.Price * cartItem.Quantity;
                totalAmount += subtotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = subtotal
                });

                // Deduct stock
                product.StockQuantity -= cartItem.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                ShippingAddress = request.ShippingAddress,
                TotalAmount = totalAmount,
                Items = orderItems
            };

            await _orderRepository.AddAsync(order);

            // Clear cart
            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Return order with included data
            var createdOrder = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return _mapper.Map<OrderResponse>(createdOrder);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PagedResult<OrderResponse>> GetMyOrdersAsync(Guid userId, int page, int size)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var totalCount = await _context.Orders
            .Where(o => o.UserId == userId)
            .CountAsync();

        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

        return new PagedResult<OrderResponse>
        {
            Items = orderResponses,
            PageNumber = page,
            PageSize = size,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<OrderResponse>> GetAllOrdersAsync(OrdersAdminQuery query)
    {
        var orders = await _orderRepository.GetAdminOrdersAsync(
            query.PageNumber, 
            query.PageSize, 
            query.UserEmail, 
            query.DateFrom, 
            query.DateTo);

        var orderResponses = _mapper.Map<List<OrderResponse>>(orders.Items);

        return new PagedResult<OrderResponse>
        {
            Items = orderResponses,
            PageNumber = orders.PageNumber,
            PageSize = orders.PageSize,
            TotalCount = orders.TotalCount
        };
    }
}
