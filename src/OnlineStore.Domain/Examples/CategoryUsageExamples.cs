using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Domain.Examples;

/// <summary>
/// Examples of how to use the CategoryIds constants for easy referencing
/// </summary>
public static class CategoryUsageExamples
{
    /// <summary>
    /// Example: Creating a new product with Electronics category (numeric ID: 1)
    /// </summary>
    public static Product CreateElectronicsProduct()
    {
        return new Product
        {
            Name = "New Smartphone",
            Description = "Latest smartphone model",
            Price = 799.99m,
            StockQuantity = 100,
            CategoryId = CategoryIds.Electronics // Easy reference to category 1
        };
    }
    
    /// <summary>
    /// Example: Creating a new product with Groceries category (numeric ID: 2)
    /// </summary>
    public static Product CreateGroceryProduct()
    {
        return new Product
        {
            Name = "Organic Bananas",
            Description = "Fresh organic bananas",
            Price = 3.99m,
            StockQuantity = 500,
            CategoryId = CategoryIds.Groceries // Easy reference to category 2
        };
    }
    
    /// <summary>
    /// Example: Creating a new product with Clothing category (numeric ID: 3)
    /// </summary>
    public static Product CreateClothingProduct()
    {
        return new Product
        {
            Name = "Designer Jacket",
            Description = "Premium designer jacket",
            Price = 199.99m,
            StockQuantity = 25,
            CategoryId = CategoryIds.Clothing // Easy reference to category 3
        };
    }
    
    /// <summary>
    /// Example: Check if a product belongs to a main category
    /// </summary>
    public static bool IsMainCategoryProduct(Product product)
    {
        return CategoryIds.IsMainCategory(product.CategoryId);
    }
    
    /// <summary>
    /// Example: Get the numeric ID representation
    /// </summary>
    public static string GetCategoryInfo(Guid categoryId)
    {
        var numericId = CategoryIds.GetNumericId(categoryId);
        if (numericId.HasValue)
        {
            return $"Main Category - Numeric ID: {numericId.Value}";
        }
        return "Custom Category";
    }
    
    /// <summary>
    /// Example: Working with all main categories
    /// </summary>
    public static void ProcessMainCategories()
    {
        foreach (var categoryId in CategoryIds.All)
        {
            var numericId = CategoryIds.GetNumericId(categoryId);
            Console.WriteLine($"Processing category with numeric ID: {numericId}");
        }
    }
}
