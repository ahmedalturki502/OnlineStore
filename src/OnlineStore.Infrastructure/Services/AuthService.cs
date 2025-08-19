using Microsoft.AspNetCore.Identity;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Services;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly JwtService _jwtService;

    public AuthService(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Assign Customer role by default
        await _userManager.AddToRoleAsync(user, "Customer");

        var token = _jwtService.GenerateToken(user, "Customer");
        var expiresAt = DateTime.UtcNow.AddMinutes(20); // Default 20 minutes

        return new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            Role = "Customer",
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Customer";

        var token = _jwtService.GenerateToken(user, role);
        var expiresAt = DateTime.UtcNow.AddMinutes(20); //  20 minutes

        return new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            Role = role,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!
        };
    }
}
