using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderErrorExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create("Plato no válido", new ApiError
            {
                Message = "El plato especificado no existe o no está disponible"
            });
            yield return SwaggerExample.Create("Cantidad inválida", new ApiError
            {
                Message = "La cantidad debe ser mayor a 0"
            });
            yield return SwaggerExample.Create("Tipo de entrega faltante", new ApiError
            {
                Message = "Debe especificar un tipo de entrega válido"
            });
        }
    }
}
