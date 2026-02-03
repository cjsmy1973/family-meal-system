using System.ComponentModel.DataAnnotations;

namespace FamilyMeal.Api.Models;

public class StepIngredient
{
    [Key]
    public int Id { get; set; }

    public int StepId { get; set; }

    public RecipeStep Step { get; set; } = null!;

    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; } = null!;

    public double Amount { get; set; }

    [MaxLength(20)]
    public string? Unit { get; set; }
}
