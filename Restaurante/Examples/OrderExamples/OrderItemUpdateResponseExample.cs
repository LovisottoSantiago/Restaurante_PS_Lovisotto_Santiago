using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderItemUpdateResponseExample : IExamplesProvider<OrderUpdateReponse>
    {
        public OrderUpdateReponse GetExamples()
        {
            return new OrderUpdateReponse
            {
                OrderNumber = 1001,
                TotalAmount = 1800.50m,
                UpdateAt = DateTime.Parse("2024-03-15T15:10:00Z")
            };
        }
    }
}
