 using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Models;
using Application.Response;

namespace Application.UseCases.DishUseCases
{
    public class GetAllDishesUseCase
    {
        private readonly IDishQuery _query;

        public GetAllDishesUseCase(IDishQuery query)
        {
            _query = query;
        }
        public async Task<IReadOnlyList<DishResponse>> ExecuteAsync(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive)
        {
            var dishes = await _query.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(name))
                dishes = dishes.Where(dish => dish.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (onlyActive)
                dishes = dishes.Where(dish => dish.Available).ToList();

            if (categoryId.HasValue)
                dishes = dishes.Where(dish => dish.Category == categoryId.Value).ToList();

            if (sortByPrice.HasValue)
            {
                if (sortByPrice == SortDirection.asc)
                    dishes = dishes.OrderBy(d => d.Price).ToList();
                else if (sortByPrice == SortDirection.desc)
                    dishes = dishes.OrderByDescending(d => d.Price).ToList();
                else
                    throw new BadRequestException400("Parámetros de ordenamiento inválidos");
            }

            return dishes.Select(d => new DishResponse
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Category = new GenericResponse
                {
                    Id = d.CategoryNavigation.Id,
                    Name = d.CategoryNavigation.Name
                },
                IsActive = d.Available,
                Image = d.ImageUrl,
                CreatedAt = d.CreateDate,
                UpdatedAt = d.UpdateDate
            }).ToList();
        }
    }
}
