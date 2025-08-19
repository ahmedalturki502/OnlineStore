using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Repositories;
using OnlineStore.Application.Interfaces.Services;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public CartService(
        ICartRepository cartRepository, 
        IProductRepository productRepository,
        IMapper mapper,
        AppDbContext context)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<CartResponse> GetAsync(Guid userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            return new CartResponse();
        }

        return _mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> AddAsync(Guid userId, AddCartItemRequest request)
    {
        // Validate product exists and has stock
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }

        if (product.StockQuantity < request.Quantity)
        {
            throw new InvalidOperationException("Insufficient stock.");
        }

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _cartRepository.AddAsync(cart);
            cart = await _cartRepository.GetByUserIdAsync(userId);
        }

        var existingItem = cart!.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
            if (existingItem.Quantity > product.StockQuantity)
            {
                throw new InvalidOperationException("Insufficient stock.");
            }
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await GetAsync(userId);
    }

    public async Task<CartResponse> UpdateAsync(Guid userId, Guid productId, int quantity)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            throw new ArgumentException("Cart not found.");
        }

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
        {
            throw new ArgumentException("Item not found in cart.");
        }

        var product = await _productRepository.GetByIdAsync(productId);
        if (product!.StockQuantity < quantity)
        {
            throw new InvalidOperationException("Insufficient stock.");
        }

        item.Quantity = quantity;
        item.UpdatedAt = DateTime.UtcNow;
        cart.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetAsync(userId);
    }

    public async Task RemoveAsync(Guid userId, Guid productId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null) return;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return;

        cart.Items.Remove(item);
        cart.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task ClearAsync(Guid userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null) return;

        cart.Items.Clear();
        cart.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
