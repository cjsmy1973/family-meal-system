namespace FamilyMeal.Api.Models;

// Ingredient DTOs
public record CreateIngredientDto(string Name, string? Category, string? Unit);
public record UpdateIngredientDto(string Name, string? Category, string? Unit);
public record IngredientDto(int Id, string Name, string? Category, string? Unit, DateTime CreatedAt);

// Condiment DTOs
public record CreateCondimentDto(string Name, string? Unit);
public record UpdateCondimentDto(string Name, string? Unit);
public record CondimentDto(int Id, string Name, string? Unit, DateTime CreatedAt);

// Recipe DTOs
public record CreateRecipeDto(string Name, string? Description, string? ImageUrl, string MealType);
public record UpdateRecipeDto(string Name, string? Description, string? ImageUrl, string MealType);
public record RecipeDto(int Id, string Name, string? Description, string? ImageUrl, string MealType, DateTime CreatedAt);

// Recipe Detail DTO
public record RecipeDetailDto(
    int Id,
    string Name,
    string? Description,
    string? ImageUrl,
    string MealType,
    DateTime CreatedAt,
    List<RecipeStepDto> Steps
);

public record RecipeStepDto(
    int Id,
    int StepOrder,
    string Description,
    string? ImageUrl,
    List<StepIngredientDto> Ingredients,
    List<StepCondimentDto> Condiments
);

public record StepIngredientDto(int IngredientId, string IngredientName, double Amount, string? Unit);
public record StepCondimentDto(int CondimentId, string CondimentName, double Amount, string? Unit);

// Recipe Step DTOs for creation
public record CreateRecipeStepDto(int StepOrder, string Description, string? ImageUrl, List<CreateStepIngredientDto> Ingredients, List<CreateStepCondimentDto> Condiments);
public record CreateStepIngredientDto(int IngredientId, double Amount, string? Unit);
public record CreateStepCondimentDto(int CondimentId, double Amount, string? Unit);

public record CreateRecipeFullDto(
    string Name,
    string? Description,
    string? ImageUrl,
    string MealType,
    List<CreateRecipeStepDto> Steps
);

// Reservation DTOs
public record CreateReservationDto(DateTime ReservationDate, string MealType, int RecipeId);
public record ReservationDto(int Id, DateTime ReservationDate, string MealType, int RecipeId, string RecipeName, DateTime CreatedAt);

// Shopping List DTOs
public record ShoppingItemDto(string Name, string Category, double TotalAmount, string Unit);
public record ShoppingListDto(List<ShoppingItemDto> Ingredients, List<ShoppingItemDto> Condiments);
