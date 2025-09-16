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
                        Id = Guid.Parse("F71F0B60-6762-4B37-9F97-09912EB9C061"),
                        Quantity = 3,
                        Notes = "Queso extra"
                    },
                    new Items
                    {
                        Id = Guid.Parse("3F5570A4-E36F-4EDD-AB3D-6292C85DF27E"),
                        Quantity = 4,
                        Notes = "Con dulce de leche repostero"
                    }
                }
            };
        }
    }
}
