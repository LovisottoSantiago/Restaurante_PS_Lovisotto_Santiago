using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Interfaces.Service;
using Application.Services;
using Application.UseCases.DishUseCases;
using Infrastructure.Commands;
using Infrastructure.Filters;
using Infrastructure.Persistence;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Restaurante.Examples.DishExamples;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


// Custom: mis inyecciones
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
    options.Filters.Add<ValidateSortByPriceFilter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    options.MapType<Application.Models.SortDirection>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
        {
            new Microsoft.OpenApi.Any.OpenApiString("asc"),
            new Microsoft.OpenApi.Any.OpenApiString("desc")
        }
    });
    options.EnableAnnotations();
    options.ExampleFilters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RestaurantAPI",
        Version = "1.0",
        Description = "API para la gestión de platos en un restaurante",
        Contact = new OpenApiContact
        {
            Name = "Restaurant API Support",
            Email = "lolivera@unaj.edu.ar"
        }
    });
});


builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorConflictExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorGetBadRequestExamples>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorNotFoundExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorPostBadRequestExamples>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorPutBadRequestExamples>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishListResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishUpdateRequestExample>();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Dependency Injection //
// Application Services
builder.Services.AddScoped<IDishService, DishService>();

// Queries & Commands
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();

// Use Cases
builder.Services.AddScoped<GetAllDishesUseCase>();
builder.Services.AddScoped<GetDishByIdUseCase>();
builder.Services.AddScoped<CreateDishUseCase>();
builder.Services.AddScoped<UpdateDishUseCase>();


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


// Necesario para los tests con WebApplicationFactory
public partial class Program { }