using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Domain.Constants;
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
            var products = new[]
            {
                new Product 
                { 
                    Name = "Smartphone", 
                    Description = "Latest model smartphone with advanced features",
                    Price = 999.99m,
                    StockQuantity = 50,
                    CategoryId = CategoryIds.Electronics, // Using numeric ID: 1
                    ImageUrl = "https://example.com/smartphone.jpg"
                },
                new Product 
                { 
                    Name = "Laptop", 
                    Description = "High-performance laptop for work and gaming",
                    Price = 1299.99m,
                    StockQuantity = 25,
                    CategoryId = CategoryIds.Electronics, // Using numeric ID: 1
                    ImageUrl = "https://example.com/laptop.jpg"
                },
                new Product 
                { 
                    Name = "T-Shirt", 
                    Description = "Comfortable cotton t-shirt",
                    Price = 29.99m,
                    StockQuantity = 100,
                    CategoryId = CategoryIds.Clothing, // Using numeric ID: 3
                    ImageUrl = "https://example.com/tshirt.jpg"
                },
                new Product 
                { 
                    Name = "Jeans", 
                    Description = "Classic denim jeans",
                    Price = 79.99m,
                    StockQuantity = 75,
                    CategoryId = CategoryIds.Clothing, // Using numeric ID: 3
                    ImageUrl = "https://example.com/jeans.jpg"
                },
                new Product 
                { 
                    Name = "Organic Apples", 
                    Description = "Fresh organic apples from local farms",
                    Price = 4.99m,
                    StockQuantity = 200,
                    CategoryId = CategoryIds.Groceries, // Using numeric ID: 2
                    ImageUrl = "https://example.com/apples.jpg"
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCategoriesAsync(AppDbContext context)
    {
        // Use predefined category IDs that represent numeric IDs 1, 2, 3
        var categories = new[]
        {
            new Category
            {
                Id = CategoryIds.Electronics, // Represents numeric ID: 1
                Name = "Electronics",
                Description = "Electronic devices, gadgets, and accessories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = CategoryIds.Groceries, // Represents numeric ID: 2
                Name = "Groceries",
                Description = "Food items, beverages, and household essentials",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = CategoryIds.Clothing, // Represents numeric ID: 3
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
