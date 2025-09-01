using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Commands;
using Infrastructure.Persistence;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Custom: mis inyecciones
// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RestaurantAPI",
        Version = "1.0",
        Description = "API para la gestión de platos en un restaurante"
    });

    // XML comments (para descripciones en Swagger UI)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Dependency Injection
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<IDishService, DishService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
