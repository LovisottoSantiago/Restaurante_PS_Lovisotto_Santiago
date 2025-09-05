using Swashbuckle.AspNetCore.Filters;
using Application.Response;

namespace Restaurante.Examples
{
    public class DishResponseExample : IExamplesProvider<DishResponse>
    {
        public DishResponse GetExamples()
        {
            return new DishResponse
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                Name = "Pizza Margherita",
                Description = "Pizza clásica con salsa de tomate, mozzarella fresca, albahaca y aceite de oliva extra virgen",
                Price = 850.50m,
                Category = new GenericResponse
                {
                    Id = 1,
                    Name = "Pizzas"
                },
                Image = "https://restaurant.com/images/pizza-margherita.jpg",
                IsActive = true,
                CreatedAt = DateTime.Parse("2024-03-15T10:30:00Z"),
                UpdatedAt = DateTime.Parse("2024-03-15T10:30:00Z")
            };
        }
    }
}
