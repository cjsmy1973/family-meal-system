namespace FamilyMeal.Api.Models;

// Ingredient DTOs
public class CreateIngredientDto
{
    public string Name { get; set; }
    public string? Category { get; set; }
    public string? Unit { get; set; }
}
public class UpdateIngredientDto
{
    public string Name { get; set; }
    public string? Category { get; set; }
    public string? Unit { get; set; }
}
public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Category { get; set; }
    public string? Unit { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Condiment DTOs
public class CreateCondimentDto
{
    public string Name { get; set; }
    public string? Unit { get; set; }
}
public class UpdateCondimentDto
{
    public string Name { get; set; }
    public string? Unit { get; set; }
}
public class CondimentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Unit { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Recipe DTOs
public class CreateRecipeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string MealType { get; set; } = string.Empty;
}
public class UpdateRecipeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string MealType { get; set; } = string.Empty;
}
public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string MealType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Recipe Detail DTO
public class RecipeDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string MealType { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RecipeStepDto> Steps { get; set; } = new();
}

public class RecipeStepDto
{
    public int Id { get; set; }
    public int StepOrder { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<StepIngredientDto> Ingredients { get; set; } = new();
    public List<StepCondimentDto> Condiments { get; set; } = new();
}

public class StepIngredientDto
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public double Amount { get; set; }
    public string? Unit { get; set; }
}

public class StepCondimentDto
{
    public int CondimentId { get; set; }
    public string CondimentName { get; set; }
    public double Amount { get; set; }
    public string? Unit { get; set; }
}

// Recipe Step DTOs for creation
public class CreateRecipeStepDto
{
    public int StepOrder { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public List<CreateStepIngredientDto> Ingredients { get; set; } = new();
    public List<CreateStepCondimentDto> Condiments { get; set; } = new();
}

public class CreateStepIngredientDto
{
    public int IngredientId { get; set; }
    public double Amount { get; set; }
    public string? Unit { get; set; }
}

public class CreateStepCondimentDto
{
    public int CondimentId { get; set; }
    public double Amount { get; set; }
    public string? Unit { get; set; }
}

public class CreateRecipeFullDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string MealType { get; set; } = string.Empty;
    public List<CreateRecipeStepDto> Steps { get; set; } = new();
}

// Recipe response DTO (already defined above)

// Reservation DTOs
public class CreateReservationDto
{
    public DateTime ReservationDate { get; set; }
    public string MealType { get; set; } = string.Empty;
    public int RecipeId { get; set; }
}
public class ReservationDto
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public string MealType { get; set; }
    public int RecipeId { get; set; }
    public string RecipeName { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Shopping List DTOs
public class ShoppingItemDto
{
    public string Name { get; set; }
    public string Category { get; set; }
    public double TotalAmount { get; set; }
    public string Unit { get; set; }
}
public class ShoppingListDto
{
    public List<ShoppingItemDto> Ingredients { get; set; } = new();
    public List<ShoppingItemDto> Condiments { get; set; } = new();
}
