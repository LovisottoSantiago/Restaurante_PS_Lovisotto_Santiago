using Swashbuckle.AspNetCore.Filters;
using Application.Models;

namespace Restaurante.Examples
{
    public class DishUpdateRequestExample : IExamplesProvider<DishUpdateRequest>
    {
        public DishUpdateRequest GetExamples()
        {
            return new DishUpdateRequest
            {
                Name = "Pizza Margherita Premium",
                Description = "Pizza artesanal con mozzarella de búfala, tomates San Marzano, albahaca orgánica y aceite de oliva extra virgen DOP",
                Price = 1200.00m,
                Category = 1,
                Image = "https://restaurant.com/images/pizza-margherita-premium.jpg",
                IsActive = true
            };
        }
    }
}
