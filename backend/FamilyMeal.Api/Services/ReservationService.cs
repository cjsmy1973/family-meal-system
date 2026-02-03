using FamilyMeal.Api.Data;
using FamilyMeal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyMeal.Api.Services;

public class ReservationService
{
    private readonly FamilyMealDbContext _context;

    public ReservationService(FamilyMealDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReservationDto>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Recipe)
            .OrderBy(r => r.ReservationDate)
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                ReservationDate = r.ReservationDate,
                MealType = r.MealType,
                RecipeId = r.RecipeId,
                RecipeName = r.Recipe.Name,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Recipe)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (reservation == null) return null;
        return new ReservationDto
        {
            Id = reservation.Id,
            ReservationDate = reservation.ReservationDate,
            MealType = reservation.MealType,
            RecipeId = reservation.RecipeId,
            RecipeName = reservation.Recipe.Name,
            CreatedAt = reservation.CreatedAt
        };
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationDto dto)
    {
        var reservation = new Reservation
        {
            ReservationDate = dto.ReservationDate,
            MealType = dto.MealType,
            RecipeId = dto.RecipeId,
            CreatedAt = DateTime.UtcNow
        };
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return new ReservationDto
        {
            Id = reservation.Id,
            ReservationDate = reservation.ReservationDate,
            MealType = reservation.MealType,
            RecipeId = reservation.RecipeId,
            RecipeName = "",
            CreatedAt = reservation.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return false;
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ShoppingListDto> GetShoppingListAsync(DateTime startDate, DateTime endDate)
    {
        var reservations = await _context.Reservations
            .Include(r => r.Recipe)
            .ThenInclude(r => r.RecipeSteps)
            .ThenInclude(s => s.StepIngredients)
            .ThenInclude(si => si.Ingredient)
            .Include(r => r.Recipe)
            .ThenInclude(r => r.RecipeSteps)
            .ThenInclude(s => s.StepCondiments)
            .ThenInclude(sc => sc.Condiment)
            .Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate)
            .ToListAsync();

        var ingredientGroups = new Dictionary<string, ShoppingItemDto>();
        var condimentGroups = new Dictionary<string, ShoppingItemDto>();

        foreach (var reservation in reservations)
        {
            foreach (var step in reservation.Recipe.RecipeSteps)
            {
                foreach (var si in step.StepIngredients)
                {
                    var key = si.Ingredient.Name;
                    if (!ingredientGroups.ContainsKey(key))
                    {
                        ingredientGroups[key] = new ShoppingItemDto
                        {
                            Name = si.Ingredient.Name,
                            Category = si.Ingredient.Category ?? "未分类",
                            TotalAmount = 0,
                            Unit = si.Unit ?? si.Ingredient.Unit ?? "g"
                        };
                    }
                    ingredientGroups[key].TotalAmount += si.Amount;
                }

                foreach (var sc in step.StepCondiments)
                {
                    var key = sc.Condiment.Name;
                    if (!condimentGroups.ContainsKey(key))
                    {
                        condimentGroups[key] = new ShoppingItemDto
                        {
                            Name = sc.Condiment.Name,
                            Category = "调味品",
                            TotalAmount = 0,
                            Unit = sc.Unit ?? sc.Condiment.Unit ?? "g"
                        };
                    }
                    condimentGroups[key].TotalAmount += sc.Amount;
                }
            }
        }

        return new ShoppingListDto
        {
            Ingredients = ingredientGroups.Values.ToList(),
            Condiments = condimentGroups.Values.ToList()
        };
    }
}
