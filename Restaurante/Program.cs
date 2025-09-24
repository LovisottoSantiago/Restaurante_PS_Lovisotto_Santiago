using Infrastructure.Filters;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Restaurante.DependencyInjection;
using Restaurante.Examples.CategoryExamples;
using Restaurante.Examples.DeliveryTypeExamples;
using Restaurante.Examples.DishExamples;
using Restaurante.Examples.OrderExamples;
using Restaurante.Examples.StatusExamples;
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
        Title = "Restaurant API",
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
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorGetBadRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorDeleteConflictExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorNotFoundExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorPostBadRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ApiErrorPutBadRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishListResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DishUpdateRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<CategoryResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<DeliveryTypeResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderCreateResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderErrorExamples>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderDetailsResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderSearchErrorExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderUpdateResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderUpdateErrorExamples>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderNotFoundExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderDetailsByIdResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderItemUpdateRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderItemUpdateResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderItemUpdateErrorExamples>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderItemNotFoundExamples>();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Dependency Injection //
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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