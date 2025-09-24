using Restaurante.Examples.CategoryExamples;
using Restaurante.Examples.DeliveryTypeExamples;
using Restaurante.Examples.DishExamples;
using Restaurante.Examples.OrderExamples;
using Restaurante.Examples.StatusExamples;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.DependencyInjection
{
    public static class ExamplesDependencyInjection
    {
        public static IServiceCollection AddExamples(this IServiceCollection services)
        {
            // ========= API ERRORS =========
            services.AddSwaggerExamplesFromAssemblyOf<ApiErrorConflictExample>();
            services.AddSwaggerExamplesFromAssemblyOf<ApiErrorGetBadRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<ApiErrorDeleteConflictExample>();
            services.AddSwaggerExamplesFromAssemblyOf<ApiErrorNotFoundExample>();
            services.AddSwaggerExamplesFromAssemblyOf<ApiErrorPostBadRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<ApiErrorPutBadRequestExample>();

            // ========= DISH =========
            services.AddSwaggerExamplesFromAssemblyOf<DishListResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<DishRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<DishResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<DishUpdateRequestExample>();

            // ========= CATEGORY =========
            services.AddSwaggerExamplesFromAssemblyOf<CategoryResponseExample>();

            // ========= DELIVERY TYPE =========
            services.AddSwaggerExamplesFromAssemblyOf<DeliveryTypeResponseExample>();

            // ========= ORDER =========
            services.AddSwaggerExamplesFromAssemblyOf<OrderRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderCreateResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderErrorExamples>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderDetailsResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderSearchErrorExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderUpdateResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderUpdateErrorExamples>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderNotFoundExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderDetailsByIdResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderItemUpdateRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderItemUpdateResponseExample>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderItemUpdateErrorExamples>();
            services.AddSwaggerExamplesFromAssemblyOf<OrderItemNotFoundExamples>();

            // ========= STATUS =========
            services.AddSwaggerExamplesFromAssemblyOf<StatusResponseExample>();

            return services;
        }
    }
}
