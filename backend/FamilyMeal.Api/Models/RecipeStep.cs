using System.ComponentModel.DataAnnotations;

namespace FamilyMeal.Api.Models;

public class RecipeStep
{
    [Key]
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;

    public int StepOrder { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    public ICollection<StepIngredient> StepIngredients { get; set; } = new List<StepIngredient>();
    public ICollection<StepCondiment> StepCondiments { get; set; } = new List<StepCondiment>();
}
