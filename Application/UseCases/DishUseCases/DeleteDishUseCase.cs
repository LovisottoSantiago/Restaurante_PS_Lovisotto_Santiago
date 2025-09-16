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

            var existsOrder = await _orderQuery.ExistsOrderWithDishAsync(dish.DishId);
            if (existsOrder)
                throw new ConflictException409("No se puede eliminar el plato porque está incluido en órdenes activas");

            await _command.DeleteAsync(dish);

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
