using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = default!;
}

public class OrderItemResponse
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
