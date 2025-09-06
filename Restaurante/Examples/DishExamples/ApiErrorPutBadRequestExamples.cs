using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    // PUT /Dish/{id} → 400
    public class ApiErrorPutBadRequestExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "invalid_price",
                new ApiError { Message = "El precio debe ser mayor a cero" }
            );
        }
    }
}
