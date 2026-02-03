using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        // Configure Ingredient
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        // Configure Condiment
        modelBuilder.Entity<Condiment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        // Configure Recipe
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MealType).HasMaxLength(20).IsRequired();
        });

        // Configure RecipeStep
        modelBuilder.Entity<RecipeStep>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.ImageUrl).HasMaxLength(255);

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.RecipeSteps)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure StepIngredient
        modelBuilder.Entity<StepIngredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(e => e.Step)
                .WithMany(s => s.StepIngredients)
                .HasForeignKey(e => e.StepId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Ingredient)
                .WithMany()
                .HasForeignKey(e => e.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure StepCondiment
        modelBuilder.Entity<StepCondiment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(e => e.Step)
                .WithMany(s => s.StepCondiments)
                .HasForeignKey(e => e.StepId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Condiment)
                .WithMany()
                .HasForeignKey(e => e.CondimentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Reservation
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Recipe)
                .WithMany()
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
