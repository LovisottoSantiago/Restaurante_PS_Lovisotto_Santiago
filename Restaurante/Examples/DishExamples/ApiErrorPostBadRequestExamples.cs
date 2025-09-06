using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    // POST /Dish → 400
    public class ApiErrorPostBadRequestExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "invalid_price",
                new ApiError { Message = "El precio debe ser mayor a cero" }
            );

            yield return SwaggerExample.Create(
                "empty_name",
                new ApiError { Message = "El nombre del plato es obligatorio" }
            );
        }
    }
}
