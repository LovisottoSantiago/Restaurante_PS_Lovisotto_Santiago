using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.UseCases.DishUseCases.UpdateDish
{
    public class UpdateDishHandler : IRequestHandler<UpdateDishCommand, DishResponse>
    {
        private readonly IDishQuery _query;
        private readonly IDishCommand _command;
        private readonly ICategoryQuery _categoryQuery;

        public UpdateDishHandler(IDishQuery dishQuery, IDishCommand dishCommand, ICategoryQuery categoryQuery)
        {
            _query = dishQuery;
            _command = dishCommand;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse> Handle(UpdateDishCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var dish = await _query.GetByIdAsync(command.Id, cancellationToken);
            if (dish is null)
                throw new NotFoundException404("Plato no encontrado");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException400("El nombre del plato es obligatorio");

            if (request.Price <= 0)
                throw new BadRequestException400("El precio debe ser mayor a cero");

            if (!await _categoryQuery.ExistsAsync(request.Category, cancellationToken))
                throw new BadRequestException400("La categoría debe existir en el sistema");

            if (await _query.ExistsByNameAsync(request.Name, command.Id, cancellationToken))
                throw new ConflictException409("Ya existe un plato con ese nombre");

            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.ImageUrl = request.Image;
            dish.Available = request.IsActive;
            dish.UpdateDate = DateTime.UtcNow;

            await _command.UpdateAsync(dish, cancellationToken);

            var updated = await _query.GetByIdAsync(dish.DishId, cancellationToken);

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
