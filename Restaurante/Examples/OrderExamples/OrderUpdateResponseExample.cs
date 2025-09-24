using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderUpdateResponseExample : IExamplesProvider<OrderUpdateReponse>
    {
        public OrderUpdateReponse GetExamples()
        {
            return new OrderUpdateReponse
            {
                OrderNumber = 1001,
                TotalAmount = 2650.75m,
                UpdateAt = DateTime.Parse("2024-03-15T14:40:00Z")
            };
        }
    }
}
