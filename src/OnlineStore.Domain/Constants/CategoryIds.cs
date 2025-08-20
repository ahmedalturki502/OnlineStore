namespace OnlineStore.Domain.Constants;

/// <summary>
/// Constants for the three main product categories with easy-to-remember numeric-based GUIDs
/// </summary>
public static class CategoryIds
{
    /// <summary>
    /// Electronics category ID (represents numeric ID: 1)
    /// </summary>
    public static readonly Guid Electronics = Guid.Parse("4E038B39-C4DE-495A-9363-C314F488D956");
    
    /// <summary>
    /// Groceries category ID (represents numeric ID: 2)
    /// </summary>
    public static readonly Guid Groceries = Guid.Parse("4E038B39-C4DE-495A-9363-C314F488D957");

    /// <summary>
    /// Clothing category ID (represents numeric ID: 3)
    /// </summary>
    public static readonly Guid Clothing = Guid.Parse("4E038B39-C4DE-495A-9363-C314F488D958");

    /// <summary>
    /// Gets all main category IDs
    /// </summary>
    public static readonly Guid[] All = { Electronics, Groceries, Clothing };
    
    /// <summary>
    /// Check if a given ID is one of the main categories
    /// </summary>
    /// <param name="categoryId">The category ID to check</param>
    /// <returns>True if it's one of the main categories</returns>
    public static bool IsMainCategory(Guid categoryId)
    {
        return categoryId == Electronics || categoryId == Groceries || categoryId == Clothing;
    }
    
    /// <summary>
    /// Get the numeric representation of the category ID
    /// </summary>
    /// <param name="categoryId">The category ID</param>
    /// <returns>The numeric ID (1, 2, 3) or null if not a main category</returns>
    public static int? GetNumericId(Guid categoryId)
    {
        if (categoryId == Electronics) return 1;
        if (categoryId == Groceries) return 2;
        if (categoryId == Clothing) return 3;
        return null;
    }
    
    /// <summary>
    /// Get category ID from numeric representation
    /// </summary>
    /// <param name="numericId">The numeric ID (1, 2, or 3)</param>
    /// <returns>The corresponding GUID or null if invalid</returns>
    public static Guid? GetGuidFromNumeric(int numericId)
    {
        return numericId switch
        {
            1 => Electronics,
            2 => Groceries,
            3 => Clothing,
            _ => null
        };
    }
}
