namespace OnlineStore.Application.DTOs;

public class CartResponse
{
    public List<CartItemResponse> Items { get; set; } = new();
    public decimal Total { get; set; }
}

public class CartItemResponse
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
