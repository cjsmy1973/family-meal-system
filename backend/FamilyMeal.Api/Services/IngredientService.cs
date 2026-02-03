using FamilyMeal.Api.Data;
using FamilyMeal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyMeal.Api.Services;

public class IngredientService
{
    private readonly FamilyMealDbContext _context;

    public IngredientService(FamilyMealDbContext context)
    {
        _context = context;
    }

    public async Task<List<IngredientDto>> GetAllAsync()
    {
        return await _context.Ingredients
            .OrderBy(i => i.CreatedAt)
            .Select(i => new IngredientDto(i.Id, i.Name, i.Category, i.Unit, i.CreatedAt))
            .ToListAsync();
    }

    public async Task<IngredientDto?> GetByIdAsync(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return null;
        return new IngredientDto(ingredient.Id, ingredient.Name, ingredient.Category, ingredient.Unit, ingredient.CreatedAt);
    }

    public async Task<IngredientDto> CreateAsync(CreateIngredientDto dto)
    {
        var ingredient = new Ingredient
        {
            Name = dto.Name,
            Category = dto.Category,
            Unit = dto.Unit,
            CreatedAt = DateTime.UtcNow
        };
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        return new IngredientDto(ingredient.Id, ingredient.Name, ingredient.Category, ingredient.Unit, ingredient.CreatedAt);
    }

    public async Task<bool> UpdateAsync(int id, UpdateIngredientDto dto)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return false;
        ingredient.Name = dto.Name;
        ingredient.Category = dto.Category;
        ingredient.Unit = dto.Unit;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return false;
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return true;
    }
}
