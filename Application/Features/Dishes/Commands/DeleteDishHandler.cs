using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Commands
{
    public class DeleteDishHandler : IRequestHandler<DeleteDishCommand, DishResponse>
    {
        private readonly IDishQuery _query;
        private readonly IDishCommand _command;
        private readonly IOrderQuery _orderQuery;

        public DeleteDishHandler(IDishQuery dishQuery, IDishCommand dishCommand, IOrderQuery orderQuery)
        {
            _query = dishQuery;
            _command = dishCommand;
            _orderQuery = orderQuery;
        }

        public async Task<DishResponse> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _query.GetByIdAsync(request.Id, cancellationToken);

            if (dish is null)
                throw new NotFoundException404("Plato no encontrado");

            var existsOrder = await _orderQuery.ExistsOrderWithDishAsync(dish.DishId, cancellationToken);
            if (existsOrder)
                throw new ConflictException409("No se puede eliminar el plato porque está incluido en órdenes activas");

            await _command.DeleteAsync(dish, cancellationToken);

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
