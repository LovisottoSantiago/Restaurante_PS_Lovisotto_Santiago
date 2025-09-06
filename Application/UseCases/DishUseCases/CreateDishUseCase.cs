using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Models;
using Application.Response;
using Domain.Entities;

namespace Application.UseCases.DishUseCases
{
    public class CreateDishUseCase
    {
        private readonly IDishQuery _query;
        private readonly IDishCommand _command;
        private readonly ICategoryQuery _categoryQuery;

        public CreateDishUseCase(IDishQuery query, IDishCommand command, ICategoryQuery categoryQuery)
        {
            _query = query;
            _command = command;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse> ExecuteAsync(DishRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException400("El nombre del plato es obligatorio");

            if (request.Price <= 0)
                throw new BadRequestException400("El precio debe ser mayor a cero");

            if (!await _categoryQuery.ExistsAsync(request.Category))
                throw new BadRequestException400("La categoría debe existir en el sistema");

            if (await _query.ExistsByNameAsync(request.Name))
                throw new ConflictException409("Ya existe un plato con ese nombre");

            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                ImageUrl = request.Image,
                Available = true,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _command.InsertAsync(dish);

            // Lo traigo de nuevo con la categoría ya incluida
            var dishCreated = await _query.GetByIdAsync(dish.DishId);

            return new DishResponse
            {
                Id = dishCreated.DishId,
                Name = dishCreated.Name,
                Description = dishCreated.Description,
                Price = dishCreated.Price,
                Category = new GenericResponse
                {
                    Id = dishCreated.CategoryNavigation.Id,
                    Name = dishCreated.CategoryNavigation.Name
                },
                IsActive = dishCreated.Available,
                Image = dishCreated.ImageUrl,
                CreatedAt = dishCreated.CreateDate,
                UpdatedAt = dishCreated.UpdateDate
            };
        }
    }
}
