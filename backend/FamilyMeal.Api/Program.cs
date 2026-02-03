using Microsoft.EntityFrameworkCore;
using FamilyMeal.Api.Data;
using FamilyMeal.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=family_meal;User=root;Password=123456;";
builder.Services.AddDbContext<FamilyMealDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Register Services
builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<CondimentService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<ReservationService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
