using System.ComponentModel.DataAnnotations;

namespace FamilyMeal.Api.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    [MaxLength(20)]
    public string MealType { get; set; } = "午餐"; // 早餐, 午餐, 晚餐

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
