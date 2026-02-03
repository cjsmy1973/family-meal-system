using FamilyMeal.Api.Data;
using FamilyMeal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyMeal.Api.Services;

public class RecipeService
{
    private readonly FamilyMealDbContext _context;

    public RecipeService(FamilyMealDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecipeDto>> GetAllAsync()
    {
        return await _context.Recipes
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new RecipeDto(r.Id, r.Name, r.Description, r.ImageUrl, r.MealType, r.CreatedAt))
            .ToListAsync();
    }

    public async Task<RecipeDto?> GetByIdAsync(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null) return null;
        return new RecipeDto(recipe.Id, recipe.Name, recipe.Description, recipe.ImageUrl, recipe.MealType, recipe.CreatedAt);
    }

    public async Task<RecipeDetailDto?> GetDetailAsync(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.RecipeSteps)
            .ThenInclude(s => s.StepIngredients)
            .ThenInclude(si => si.Ingredient)
            .Include(r => r.RecipeSteps)
            .ThenInclude(s => s.StepCondiments)
            .ThenInclude(sc => sc.Condiment)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) return null;

        var steps = recipe.RecipeSteps
            .OrderBy(s => s.StepOrder)
            .Select(s => new RecipeStepDto(
                s.Id,
                s.StepOrder,
                s.Description,
                s.ImageUrl,
                s.StepIngredients.Select(si => new StepIngredientDto(
                    si.IngredientId,
                    si.Ingredient.Name,
                    si.Amount,
                    si.Unit
                )).ToList(),
                s.StepCondiments.Select(sc => new StepCondimentDto(
                    sc.CondimentId,
                    sc.Condiment.Name,
                    sc.Amount,
                    sc.Unit
                )).ToList()
            )).ToList();

        return new RecipeDetailDto(
            recipe.Id,
            recipe.Name,
            recipe.Description,
            recipe.ImageUrl,
            recipe.MealType,
            recipe.CreatedAt,
            steps
        );
    }

    public async Task<RecipeDto> CreateAsync(CreateRecipeFullDto dto)
    {
        var recipe = new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            MealType = dto.MealType,
            CreatedAt = DateTime.UtcNow
        };

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        if (dto.Steps != null)
        {
            foreach (var stepDto in dto.Steps)
            {
                var step = new RecipeStep
                {
                    RecipeId = recipe.Id,
                    StepOrder = stepDto.StepOrder,
                    Description = stepDto.Description,
                    ImageUrl = stepDto.ImageUrl
                };
                _context.RecipeSteps.Add(step);
                await _context.SaveChangesAsync();

                foreach (var ingDto in stepDto.Ingredients)
                {
                    _context.StepIngredients.Add(new StepIngredient
                    {
                        StepId = step.Id,
                        IngredientId = ingDto.IngredientId,
                        Amount = ingDto.Amount,
                        Unit = ingDto.Unit
                    });
                }

                foreach (var condDto in stepDto.Condiments)
                {
                    _context.StepCondiments.Add(new StepCondiment
                    {
                        StepId = step.Id,
                        CondimentId = condDto.CondimentId,
                        Amount = condDto.Amount,
                        Unit = condDto.Unit
                    });
                }
            }
            await _context.SaveChangesAsync();
        }

        return new RecipeDto(recipe.Id, recipe.Name, recipe.Description, recipe.ImageUrl, recipe.MealType, recipe.CreatedAt);
    }

    public async Task<bool> UpdateAsync(int id, UpdateRecipeDto dto)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null) return false;
        recipe.Name = dto.Name;
        recipe.Description = dto.Description;
        recipe.ImageUrl = dto.ImageUrl;
        recipe.MealType = dto.MealType;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null) return false;
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return true;
    }
}
