using FamilyMeal.Api.Data;
using FamilyMeal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyMeal.Api.Services;

public class CondimentService
{
    private readonly FamilyMealDbContext _context;

    public CondimentService(FamilyMealDbContext context)
    {
        _context = context;
    }

    public async Task<List<CondimentDto>> GetAllAsync()
    {
        return await _context.Condiments
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CondimentDto(c.Id, c.Name, c.Unit, c.CreatedAt))
            .ToListAsync();
    }

    public async Task<CondimentDto?> GetByIdAsync(int id)
    {
        var condiment = await _context.Condiments.FindAsync(id);
        if (condiment == null) return null;
        return new CondimentDto(condiment.Id, condiment.Name, condiment.Unit, condiment.CreatedAt);
    }

    public async Task<CondimentDto> CreateAsync(CreateCondimentDto dto)
    {
        var condiment = new Condiment
        {
            Name = dto.Name,
            Unit = dto.Unit,
            CreatedAt = DateTime.UtcNow
        };
        _context.Condiments.Add(condiment);
        await _context.SaveChangesAsync();
        return new CondimentDto(condiment.Id, condiment.Name, condiment.Unit, condiment.CreatedAt);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCondimentDto dto)
    {
        var condiment = await _context.Condiments.FindAsync(id);
        if (condiment == null) return false;
        condiment.Name = dto.Name;
        condiment.Unit = dto.Unit;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var condiment = await _context.Condiments.FindAsync(id);
        if (condiment == null) return false;
        _context.Condiments.Remove(condiment);
        await _context.SaveChangesAsync();
        return true;
    }
}
