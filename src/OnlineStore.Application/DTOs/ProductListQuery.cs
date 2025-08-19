namespace OnlineStore.Application.DTOs;

public class ProductListQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public Guid? CategoryId { get; set; }
    public string? Q { get; set; }
}
