using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Infrastructure.Commands;
using Infrastructure.Persistence;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Restaurante.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Commands & Queries
            services.AddScoped<IDishQuery, DishQuery>();
            services.AddScoped<IDishCommand, DishCommand>();
            services.AddScoped<ICategoryQuery, CategoryQuery>();
            services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
            services.AddScoped<IStatusQuery, StatusQuery>();
            services.AddScoped<IOrderCommand, OrderCommand>();
            services.AddScoped<IOrderQuery, OrderQuery>();
            services.AddScoped<IOrderItemCommand, OrderItemCommand>();

            return services;
        }
    }
}
