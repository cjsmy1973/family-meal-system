using System.ComponentModel.DataAnnotations;

namespace FamilyMeal.Api.Models;

public class StepCondiment
{
    [Key]
    public int Id { get; set; }

    public int StepId { get; set; }

    public RecipeStep Step { get; set; } = null!;

    public int CondimentId { get; set; }

    public Condiment Condiment { get; set; } = null!;

    public double Amount { get; set; }

    [MaxLength(20)]
    public string? Unit { get; set; }
}
