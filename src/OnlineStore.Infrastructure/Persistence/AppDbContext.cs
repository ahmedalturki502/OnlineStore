using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Persistence;

public sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        //  Seed Roles
        var adminRole = new AppRole { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" };
        var customerRole = new AppRole { Id = Guid.NewGuid(), Name = "Customer", NormalizedName = "CUSTOMER" };

        builder.Entity<AppRole>().HasData(adminRole, customerRole);

        //  Seed Admin User
        var hasher = new PasswordHasher<AppUser>();
        var adminUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = "admin@onlinestore.com",
            NormalizedUserName = "ADMIN@ONLINESTORE.COM",
            Email = "admin@onlinestore.com",
            NormalizedEmail = "ADMIN@ONLINESTORE.COM",
            EmailConfirmed = true,
            FullName = "Store Admin",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

        builder.Entity<AppUser>().HasData(adminUser);

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });


        builder.Entity<Category>().HasData(
    new Category
    {
        Id = new Guid("00000000-0000-0000-0000-000000000001"),
        Name = "Electronics",
        Description = "Electronic devices, gadgets, and accessories",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Category
    {
        Id = new Guid("00000000-0000-0000-0000-000000000002"),
        Name = "Groceries",
        Description = "Food items, beverages, and household essentials",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Category
    {
        Id = new Guid("00000000-0000-0000-0000-000000000003"),
        Name = "Clothing",
        Description = "Apparel, shoes, and fashion accessories",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    }
);

    }
}
