using Swashbuckle.AspNetCore.Filters;
using Application.Response;

namespace Restaurante.Examples
{
    public class DishListResponseExample : IExamplesProvider<IEnumerable<DishResponse>>
    {
        public IEnumerable<DishResponse> GetExamples()
        {
            return new List<DishResponse>
            {
                new DishResponse
                {
                    Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                    Name = "Pizza Margherita",
                    Description = "Pizza clásica con salsa de tomate, mozzarella fresca, albahaca y aceite de oliva extra virgen",
                    Price = 850.50m,
                    Category = new GenericResponse { Id = 1, Name = "Pizzas" },
                    Image = "https://restaurant.com/images/pizza-margherita.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.Parse("2024-03-15T10:30:00Z"),
                    UpdatedAt = DateTime.Parse("2024-03-15T10:30:00Z")
                },
                new DishResponse
                {
                    Id = Guid.Parse("987fcdeb-51a2-43d7-8f9e-123456789abc"),
                    Name = "Lasagna Bolognesa",
                    Description = "Lasagna tradicional con salsa bolognesa, bechamel y queso parmesano",
                    Price = 950.00m,
                    Category = new GenericResponse { Id = 2, Name = "Pastas" },
                    Image = "https://restaurant.com/images/lasagna-bolognesa.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.Parse("2024-03-15T09:15:00Z"),
                    UpdatedAt = DateTime.Parse("2024-03-15T09:15:00Z")
                }
            };
        }
    }
}
