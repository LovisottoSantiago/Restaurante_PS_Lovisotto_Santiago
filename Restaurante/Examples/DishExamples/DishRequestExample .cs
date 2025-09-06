using Application.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.DishExamples
{
    public class DishRequestExample : IExamplesProvider<DishRequest>
    {
        public DishRequest GetExamples()
        {
            return new DishRequest
            {
                Name = "Pizza Margherita",
                Description = "Pizza clásica con salsa de tomate, mozzarella fresca, albahaca y aceite de oliva extra virgen",
                Price = 850.50m,
                Category = 1,
                Image = "https://restaurant.com/images/pizza-margherita.jpg"
            };
        }
    }
}
