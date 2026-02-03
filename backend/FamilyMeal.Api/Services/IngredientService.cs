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
            .Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name,
                Category = i.Category,
                Unit = i.Unit,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<IngredientDto?> GetByIdAsync(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return null;
        return new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Category = ingredient.Category,
            Unit = ingredient.Unit,
            CreatedAt = ingredient.CreatedAt
        };
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
        return new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Category = ingredient.Category,
            Unit = ingredient.Unit,
            CreatedAt = ingredient.CreatedAt
        };
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
