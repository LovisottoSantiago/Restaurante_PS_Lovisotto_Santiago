using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    // GET /Dish → 400
    public class ApiErrorGetBadRequestExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "invalid_sort",
                new ApiError { Message = "Parámetros de ordenamiento inválidos" }
            );
        }
    }
}
