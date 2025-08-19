using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Identity;

public static class SampleDataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Seed the three main categories with fixed GUIDs
        await SeedCategoriesAsync(context);
    }

    public static async Task SeedSampleDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Seed the three main categories with fixed GUIDs
        await SeedCategoriesAsync(context);

        // Seed products if they don't exist

        if (!context.Products.Any())
        {
            var electronicsCategory = context.Categories.First(c => c.Name == "Electronics");
            var clothingCategory = context.Categories.First(c => c.Name == "Clothing");
            var groceriesCategory = context.Categories.First(c => c.Name == "Groceries");

            var products = new[]
            {
                new Product 
                { 
                    Name = "Smartphone", 
                    Description = "Latest model smartphone with advanced features",
                    Price = 999.99m,
                    StockQuantity = 50,
                    CategoryId = electronicsCategory.Id,
                    ImageUrl = "https://example.com/smartphone.jpg"
                },
                new Product 
                { 
                    Name = "Laptop", 
                    Description = "High-performance laptop for work and gaming",
                    Price = 1299.99m,
                    StockQuantity = 25,
                    CategoryId = electronicsCategory.Id,
                    ImageUrl = "https://example.com/laptop.jpg"
                },
                new Product 
                { 
                    Name = "T-Shirt", 
                    Description = "Comfortable cotton t-shirt",
                    Price = 29.99m,
                    StockQuantity = 100,
                    CategoryId = clothingCategory.Id,
                    ImageUrl = "https://example.com/tshirt.jpg"
                },
                new Product 
                { 
                    Name = "Jeans", 
                    Description = "Classic denim jeans",
                    Price = 79.99m,
                    StockQuantity = 75,
                    CategoryId = clothingCategory.Id,
                    ImageUrl = "https://example.com/jeans.jpg"
                },
                new Product 
                { 
                    Name = "Organic Apples", 
                    Description = "Fresh organic apples from local farms",
                    Price = 4.99m,
                    StockQuantity = 200,
                    CategoryId = groceriesCategory.Id,
                    ImageUrl = "https://example.com/apples.jpg"
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCategoriesAsync(AppDbContext context)
    {
        // Define fixed Guid values for reliable referencing
        var electronicsId = new Guid("11111111-1111-1111-1111-111111111111");
        var groceriesId = new Guid("22222222-2222-2222-2222-222222222222");
        var clothingId = new Guid("33333333-3333-3333-3333-333333333333");

        var categories = new[]
        {
            new Category
            {
                Id = electronicsId,
                Name = "Electronics",
                Description = "Electronic devices, gadgets, and accessories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = groceriesId,
                Name = "Groceries",
                Description = "Food items, beverages, and household essentials",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = clothingId,
                Name = "Clothing",
                Description = "Apparel, shoes, and fashion accessories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        foreach (var category in categories)
        {
            // Check if category already exists
            var existingCategory = await context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
            {
                context.Categories.Add(category);
            }
        }

        await context.SaveChangesAsync();
    }
}
