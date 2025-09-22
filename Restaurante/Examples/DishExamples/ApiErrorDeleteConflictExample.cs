using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    public class ApiErrorDeleteConflictExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "No se puede eliminar el plato porque está incluido en órdenes activas"
            };
        }
    }
}
