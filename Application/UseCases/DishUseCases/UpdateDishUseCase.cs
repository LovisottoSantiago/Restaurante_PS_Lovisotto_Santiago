using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Models;
using Application.Response;

namespace Application.UseCases.DishUseCases
{
    public class UpdateDishUseCase
    {
        private readonly IDishQuery _query;
        private readonly IDishCommand _command;
        private readonly ICategoryQuery _categoryQuery;

        public UpdateDishUseCase(IDishQuery query, IDishCommand command, ICategoryQuery categoryQuery)
        {
            _query = query;
            _command = command;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse> ExecuteAsync(Guid id, DishUpdateRequest request)
        {
            var dish = await _query.GetByIdAsync(id);

            if (dish is null)
                throw new NotFoundException404("Plato no encontrado");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException400("El nombre del plato es obligatorio");

            if (request.Price <= 0)
                throw new BadRequestException400("El precio debe ser mayor a cero");

            if (!await _categoryQuery.ExistsAsync(request.Category))
                throw new BadRequestException400("La categoría debe existir en el sistema");

            if (await _query.ExistsByNameAsync(request.Name, id))
                throw new ConflictException409("Ya existe un plato con ese nombre");

            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.ImageUrl = request.Image;
            dish.Available = request.IsActive;
            dish.UpdateDate = DateTime.UtcNow;

            await _command.UpdateAsync(dish);

            var updated = await _query.GetByIdAsync(dish.DishId);

            return new DishResponse
            {
                Id = updated.DishId,
                Name = updated.Name,
                Description = updated.Description,
                Price = updated.Price,
                Category = new GenericResponse
                {
                    Id = updated.CategoryNavigation.Id,
                    Name = updated.CategoryNavigation.Name
                },
                IsActive = updated.Available,
                Image = updated.ImageUrl,
                CreatedAt = updated.CreateDate,
                UpdatedAt = updated.UpdateDate
            };
        }
    }
}
