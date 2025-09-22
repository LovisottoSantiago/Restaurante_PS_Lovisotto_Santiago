using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{    
    public class ApiErrorGetByIdBadRequestExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "Formato de ID inválido"
            };
        }
    }
}
