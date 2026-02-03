using Microsoft.EntityFrameworkCore;
using FamilyMeal.Api.Models;

namespace FamilyMeal.Api.Data;

public class FamilyMealDbContext : DbContext
{
    public FamilyMealDbContext(DbContextOptions<FamilyMealDbContext> options) : base(options) { }

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Condiment> Condiments => Set<Condiment>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();
    public DbSet<StepIngredient> StepIngredients => Set<StepIngredient>();
    public DbSet<StepCondiment> StepCondiments => Set<StepCondiment>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RecipeStep>()
            .HasOne(s => s.Recipe)
            .WithMany(r => r.RecipeSteps)
            .HasForeignKey(s => s.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StepIngredient>()
            .HasOne(si => si.Step)
            .WithMany(s => s.StepIngredients)
            .HasForeignKey(si => si.StepId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StepIngredient>()
            .HasOne(si => si.Ingredient)
            .WithMany()
            .HasForeignKey(si => si.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StepCondiment>()
            .HasOne(sc => sc.Step)
            .WithMany(s => s.StepCondiments)
            .HasForeignKey(sc => sc.StepId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StepCondiment>()
            .HasOne(sc => sc.Condiment)
            .WithMany()
            .HasForeignKey(sc => sc.CondimentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Recipe)
            .WithMany()
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
