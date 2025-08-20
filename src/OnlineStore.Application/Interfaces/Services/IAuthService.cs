using System.Security.Claims;
using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task LogoutAsync(string refreshToken);
    Task<ProfileResponse> GetProfileAsync(ClaimsPrincipal userClaims);
    Task<ProfileResponse> UpdateProfileAsync(ClaimsPrincipal userClaims, UpdateProfileRequest request);
    Task ChangePasswordAsync(ClaimsPrincipal userClaims, ChangePasswordRequest request);
}
