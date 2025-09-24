using Infrastructure.Filters;
using System.Reflection;

namespace Restaurante.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.Load("Application"))
            );

            // Filters
            services.AddScoped<OrderItemValidationFilter>();
            return services;
        }
    }
}
