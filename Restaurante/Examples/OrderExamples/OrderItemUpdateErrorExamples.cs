using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderItemUpdateErrorExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create("Estado inválido", new ApiError
            {
                Message = "El estado especificado no es válido"
            });

            yield return SwaggerExample.Create("Transición no permitida", new ApiError
            {
                Message = "No se puede cambiar de 'Entregado' a 'En preparación'"
            });
        }
    }
}
