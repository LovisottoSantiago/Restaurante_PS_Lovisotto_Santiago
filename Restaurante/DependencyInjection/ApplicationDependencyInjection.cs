using Application.Interfaces.Service;
using Application.Services;
using Application.UseCases.CategoryUseCases;
using Application.UseCases.DeliveryTypeUseCases;
using Application.UseCases.DishUseCases;
using Application.UseCases.OrderUseCases;
using Application.UseCases.StatusUseCases;
using Infrastructure.Filters;

namespace Restaurante.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDeliveryTypeService, DeliveryTypeService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<OrderItemValidationFilter>();


            // UseCases
            services.AddScoped<GetAllDishesUseCase>();
            services.AddScoped<GetDishByIdUseCase>();
            services.AddScoped<CreateDishUseCase>();
            services.AddScoped<UpdateDishUseCase>();
            services.AddScoped<DeleteDishUseCase>();
            services.AddScoped<GetAllCategoriesUseCase>();
            services.AddScoped<GetAllDeliveryTypesUseCase>();
            services.AddScoped<GetAllStatusesUseCase>();
            services.AddScoped<CreateOrderUseCase>();
            services.AddScoped<GetAllOrdersUseCase>();
            services.AddScoped<GetOrderByIdUseCase>();
            services.AddScoped<UpdateOrderUseCase>();
            services.AddScoped<UpdateOrderItemUseCase>();

            return services;
        }
    }
}
