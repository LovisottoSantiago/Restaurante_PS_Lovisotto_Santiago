using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.DishUseCases
{
    public class DeleteDishUseCase
    {
        private readonly IDishQuery _query;
        private readonly IDishCommand _command;
        private readonly IOrderQuery _orderQuery;
        public DeleteDishUseCase(IDishQuery query, IDishCommand command, IOrderQuery orderQuery) 
        {
            _query = query;
            _command = command;
            _orderQuery = orderQuery;
        }
        public async Task<DishResponse> ExecuteAsync(Guid id)
        {
            var dish = await _query.GetByIdAsync(id);

            if (dish is null)
                throw new NotFoundException404("Plato no encontrado");

            // Validar si el plato está en órdenes activas
            var hasActiveOrders = await _orderQuery.ExistsActiveOrderWithDishAsync(dish.DishId);
            if (hasActiveOrders)
                throw new ConflictException409("No se puede eliminar el plato porque está incluido en órdenes activas");

            // Soft delete
            dish.Available = false;
            dish.UpdateDate = DateTime.UtcNow;
            await _command.SoftDeleteAsync(dish);

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
