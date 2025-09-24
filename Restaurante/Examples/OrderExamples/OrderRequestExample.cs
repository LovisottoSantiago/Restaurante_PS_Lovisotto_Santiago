using Application.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderRequestExample : IExamplesProvider<OrderRequest>
    {
        public OrderRequest GetExamples()
        {
            return new OrderRequest
            {
                Items = new List<Items>
                {
                    new Items
                    {
                        Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                        Quantity = 2,
                        Notes = "Sin albahaca, por favor"
                    },
                    new Items
                    {
                        Id = Guid.Parse("987fcdeb-51a2-43d7-8f9e-123456789abc"),
                        Quantity = 1,
                        Notes = "Bien cocida"
                    }
                },
                Delivery = new Delivery
                {
                    Id = 1,
                    To = "Av. Corrientes 1234, Buenos Aires"
                },
                Notes = "Timbre: Departamento 5B"
            };
        }
    }
}
