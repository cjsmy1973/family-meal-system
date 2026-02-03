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
            .Select(r => new ReservationDto(r.Id, r.ReservationDate, r.MealType, r.RecipeId, r.Recipe.Name, r.CreatedAt))
            .ToListAsync();
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Recipe)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (reservation == null) return null;
        return new ReservationDto(reservation.Id, reservation.ReservationDate, reservation.MealType, reservation.RecipeId, reservation.Recipe.Name, reservation.CreatedAt);
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
        return new ReservationDto(reservation.Id, reservation.ReservationDate, reservation.MealType, reservation.RecipeId, "", reservation.CreatedAt);
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
                        ingredientGroups[key] = new ShoppingItemDto(
                            si.Ingredient.Name,
                            si.Ingredient.Category ?? "未分类",
                            0,
                            si.Unit ?? si.Ingredient.Unit ?? "g"
                        );
                    }
                    ingredientGroups[key] = ingredientGroups[key] with { TotalAmount = ingredientGroups[key].TotalAmount + si.Amount };
                }

                foreach (var sc in step.StepCondiments)
                {
                    var key = sc.Condiment.Name;
                    if (!condimentGroups.ContainsKey(key))
                    {
                        condimentGroups[key] = new ShoppingItemDto(
                            sc.Condiment.Name,
                            "调味品",
                            0,
                            sc.Unit ?? sc.Condiment.Unit ?? "g"
                        );
                    }
                    condimentGroups[key] = condimentGroups[key] with { TotalAmount = condimentGroups[key].TotalAmount + sc.Amount };
                }
            }
        }

        return new ShoppingListDto(
            ingredientGroups.Values.ToList(),
            condimentGroups.Values.ToList()
        );
    }
}
