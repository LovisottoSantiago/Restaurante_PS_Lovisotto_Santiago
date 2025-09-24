using Application.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Examples.CategoryExamples
{
    public class CategoryResponseExample : IExamplesProvider<IReadOnlyList<CategoryResponse>>
    {
        public IReadOnlyList<CategoryResponse> GetExamples()
        {
            return new List<CategoryResponse>
            {
                new CategoryResponse { Id = 1, Name = "Pizzas", Description = "Pizzas artesanales con masa tradicional", Order = 1 },
                new CategoryResponse { Id = 2, Name = "Pastas", Description = "Pastas frescas caseras", Order = 2 },
                new CategoryResponse { Id = 3, Name = "Ensaladas", Description = "Ensaladas frescas y saludables", Order = 3 },
                new CategoryResponse { Id = 4, Name = "Postres", Description = "Postres caseros y dulces tradicionales", Order = 4 }
            };
        }
    }
}
