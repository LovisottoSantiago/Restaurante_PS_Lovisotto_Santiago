using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    public class ApiErrorGetByIdBadRequestExamples : IMultipleExamplesProvider<ApiError>
    {
        public IEnumerable<SwaggerExample<ApiError>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "invalid_sort",
                new ApiError { Message = "Formato de ID inválido" }
            );
        }
    }
}
