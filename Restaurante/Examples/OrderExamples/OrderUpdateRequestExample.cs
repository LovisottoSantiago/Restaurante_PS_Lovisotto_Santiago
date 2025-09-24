using Application.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderUpdateRequestExample : IExamplesProvider<OrderUpdateRequest>
    {
        public OrderUpdateRequest GetExamples()
        {
            return new OrderUpdateRequest
            {
                Items = new List<Items>
                {
                    new Items
                    {
                        Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                        Quantity = 3,
                        Notes = "Sin albahaca, extra queso"
                    },
                    new Items
                    {
                        Id = Guid.Parse("456e7890-a12b-34c5-d678-901234567890"),
                        Quantity = 1,
                        Notes = "Para compartir"
                    }
                }
            };
        }
    }
}
