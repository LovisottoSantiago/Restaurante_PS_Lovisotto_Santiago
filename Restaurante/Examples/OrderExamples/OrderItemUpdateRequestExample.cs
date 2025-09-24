using Application.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderItemUpdateRequestExample : IExamplesProvider<OrderItemUpdateRequest>
    {
        public OrderItemUpdateRequest GetExamples()
        {
            return new OrderItemUpdateRequest
            {
                Status = 3 
            };
        }
    }
}
