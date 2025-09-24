using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderItemUpdateErrorExamples : IExamplesProvider<ApiError>
    {
        public ApiError GetExamples()
        {
            return new ApiError
            {                
                Message = "No se puede cambiar un item de 'Entregado' a 'En preparación'"
            };
        }
    }
}
