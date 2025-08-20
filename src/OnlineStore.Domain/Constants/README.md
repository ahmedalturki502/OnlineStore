# Category ID Constants

This folder contains constants for easy referencing of the three main product categories using simple numeric representations.

## Overview

Instead of using long GUID values, you can now reference the three main categories using these convenient constants:

- **Electronics** → Numeric ID: 1 → `CategoryIds.Electronics`
- **Groceries** → Numeric ID: 2 → `CategoryIds.Groceries`  
- **Clothing** → Numeric ID: 3 → `CategoryIds.Clothing`

## Usage Examples

### Creating Products

```csharp
using OnlineStore.Domain.Constants;

// Create an electronics product (category 1)
var smartphone = new Product
{
    Name = "iPhone 15",
    Price = 999.99m,
    CategoryId = CategoryIds.Electronics // Instead of a long GUID
};

// Create a grocery product (category 2)
var apples = new Product
{
    Name = "Organic Apples",
    Price = 4.99m,
    CategoryId = CategoryIds.Groceries
};

// Create a clothing product (category 3)
var tshirt = new Product
{
    Name = "Cotton T-Shirt",
    Price = 19.99m,
    CategoryId = CategoryIds.Clothing
};
```

### Checking Category Types

```csharp
// Check if a product belongs to one of the main categories
bool isMainCategory = CategoryIds.IsMainCategory(product.CategoryId);

// Get the numeric representation
int? numericId = CategoryIds.GetNumericId(product.CategoryId);
if (numericId.HasValue)
{
    Console.WriteLine($"This is main category #{numericId}");
}

// Convert from numeric to GUID
Guid? categoryGuid = CategoryIds.GetGuidFromNumeric(2); // Returns Groceries GUID
```

### Working with All Main Categories

```csharp
// Process all main categories
foreach (var categoryId in CategoryIds.All)
{
    var numericId = CategoryIds.GetNumericId(categoryId);
    Console.WriteLine($"Processing category #{numericId}");
}
```

## Technical Details

- The GUID values are structured to make the numeric reference clear:
  - Electronics: `00000000-0000-0000-0000-000000000001`
  - Groceries: `00000000-0000-0000-0000-000000000002`
  - Clothing: `00000000-0000-0000-0000-000000000003`

- These constants are automatically seeded into the database when the application starts
- All existing code continues to work without changes
- The BaseEntity still uses Guid as the primary key type
- Only these three specific categories use the numeric-based GUIDs

## Benefits

1. **Easy to Remember**: Use 1, 2, 3 instead of long GUIDs
2. **Type Safe**: Still uses proper Guid types for database consistency
3. **Backward Compatible**: All existing code continues to work
4. **Convenient**: Helper methods for conversion and validation
5. **Consistent**: Categories are always seeded with the same IDs
