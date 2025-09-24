using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderItemNotFoundExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create("Orden no encontrada", new ApiError
            {
                Message = "Orden no encontrada"
            });

            yield return SwaggerExample.Create("Item no encontrado", new ApiError
            {
                Message = "Item no encontrado en la orden"
            });
        }
    }
}
