using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    // GET /Dish → 400
    public class ApiErrorGetBadRequestExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "Parámetros de ordenamiento inválidos"
            };
        }
    }
}
