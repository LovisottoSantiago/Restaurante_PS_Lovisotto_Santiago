using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderNotFoundExample : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {
                Message = "Orden no encontrada"
            };
        }
    }
}
