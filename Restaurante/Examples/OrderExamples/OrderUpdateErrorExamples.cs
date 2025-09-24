using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderUpdateErrorExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create("Orden en preparación", new ApiError
            {
                Message = "No se puede modificar una orden que ya está en preparación"
            });

            yield return SwaggerExample.Create("Plato no disponible", new ApiError
            {
                Message = "El plato especificado no está disponible"
            });
        }
    }
}
