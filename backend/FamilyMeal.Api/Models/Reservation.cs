using System.ComponentModel.DataAnnotations;

namespace FamilyMeal.Api.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    public DateTime ReservationDate { get; set; }

    [MaxLength(20)]
    public string MealType { get; set; } = "午餐"; // 早餐, 午餐, 晚餐

    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
