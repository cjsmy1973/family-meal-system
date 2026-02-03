using System.ComponentModel.DataAnnotations;

namespace FamilyMeal.Api.Models;

public class Condiment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<StepCondiment> StepCondiments { get; set; } = new List<StepCondiment>();
}
