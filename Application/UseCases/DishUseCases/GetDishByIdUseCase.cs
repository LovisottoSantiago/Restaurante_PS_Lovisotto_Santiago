using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.DishUseCases
{
    public class GetDishByIdUseCase
    {
        private readonly IDishQuery _query;

        public GetDishByIdUseCase(IDishQuery query)
        {
            _query = query;
        }

        public async Task<DishResponse?> ExecuteAsync(Guid id)
        {
            var dish = await _query.GetByIdAsync(id);
            if (dish == null)
                throw new NotFoundException404("Plato no encontrado");

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = dish.CategoryNavigation.Id,
                    Name = dish.CategoryNavigation.Name
                },
                IsActive = dish.Available,
                Image = dish.ImageUrl,
                CreatedAt = dish.CreateDate,
                UpdatedAt = dish.UpdateDate
            };
        }
    }
}
