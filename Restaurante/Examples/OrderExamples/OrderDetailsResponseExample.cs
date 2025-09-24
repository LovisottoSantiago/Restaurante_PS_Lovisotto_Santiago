using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.OrderExamples
{
    public class OrderDetailsResponseExample : IExamplesProvider<IReadOnlyList<OrderDetailsResponse>>
    {
        public IReadOnlyList<OrderDetailsResponse> GetExamples()
        {
            return new List<OrderDetailsResponse>
            {
                new OrderDetailsResponse
                {
                    OrderNumber = 1001,
                    TotalAmount = 1800.50m,
                    DeliveryTo = "Av. Corrientes 1234, Buenos Aires",
                    Notes = "Timbre: Departamento 5B",
                    Status = new GenericResponse { Id = 2, Name = "En preparación" },
                    DeliveryType = new GenericResponse { Id = 1, Name = "Delivery" },
                    Items = new List<OrderItemResponse>
                    {
                        new OrderItemResponse
                        {
                            Id = 1,
                            Quantity = 2,
                            Notes = "Sin albahaca, por favor",
                            Status = new GenericResponse { Id = 2, Name = "En preparación" },
                            Dish = new DishShortResponse
                            {
                                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                                Name = "Pizza Margherita",
                                Image = "https://restaurant.com/images/pizza-margherita.jpg"
                            }
                        },
                        new OrderItemResponse
                        {
                            Id = 2,
                            Quantity = 1,
                            Notes = "Bien cocida",
                            Status = new GenericResponse { Id = 2, Name = "En preparación" },
                            Dish = new DishShortResponse
                            {
                                Id = Guid.Parse("987fcdeb-51a2-43d7-8f9e-123456789abc"),
                                Name = "Lasagna Bolognesa",
                                Image = "https://restaurant.com/images/lasagna-bolognesa.jpg"
                            }
                        }
                    },
                    CreatedAt = DateTime.Parse("2024-03-15T14:30:00Z"),
                    UpdatedAt = DateTime.Parse("2024-03-15T14:35:00Z")
                }
            };
        }
    }
}
