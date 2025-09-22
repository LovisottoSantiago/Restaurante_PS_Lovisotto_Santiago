using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    // PUT /Dish/{id} → 400
    public class ApiErrorPutBadRequestExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "El precio debe ser mayor a cero"
            };
        }
    }
}
