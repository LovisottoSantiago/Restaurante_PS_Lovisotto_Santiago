using Swashbuckle.AspNetCore.Filters;
using Application.Response;

namespace Restaurante.Examples
{
    public class ApiErrorExamples : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "El precio debe ser mayor a cero"
            };
        }
    }

    public class ApiErrorConflictExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "Ya existe un plato con ese nombre"
            };
        }
    }

    public class ApiErrorNotFoundExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "Plato no encontrado"
            };
        }
    }
}
