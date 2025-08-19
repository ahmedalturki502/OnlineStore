namespace OnlineStore.Application.DTOs;

public class CreateProductRequest
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
}
