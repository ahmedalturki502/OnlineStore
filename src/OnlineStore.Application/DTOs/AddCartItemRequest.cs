namespace OnlineStore.Application.DTOs;

public class AddCartItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
