namespace OnlineStore.Application.DTOs;

public class UpdateProfileRequest
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
