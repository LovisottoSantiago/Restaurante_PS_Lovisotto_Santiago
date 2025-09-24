using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderCreateResponseExample : IExamplesProvider<OrderCreateReponse>
    {
        public OrderCreateReponse GetExamples()
        {
            return new OrderCreateReponse
            {
                OrderNumber = 1001,
                TotalAmount = 1800.50m,
                CreatedAt = DateTime.Parse("2024-03-15T14:30:00Z")
            };
        }
    }
}
