using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Models;
using Application.Response;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.OrderUseCases
{
    public class UpdateOrderUseCase
    {
        private readonly IOrderCommand _command;
        private readonly IOrderQuery _orderQuery;
        private readonly IDishQuery _dishQuery;

        public UpdateOrderUseCase(IOrderCommand command, IOrderQuery query, IDishQuery dishQuery)
        {
            _command = command;
            _orderQuery = query;
            _dishQuery = dishQuery;
        }

        public async Task<OrderUpdateResponse> ExecuteAsync(long orderId, OrderUpdateRequest request)
        {
            var order = await _orderQuery.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException404("La orden no existe");

            if (order.OverallStatus == (int)OrderStatus.Closed) 
                throw new BadRequestException400("No se puede modificar una orden cerrada");

            decimal total = 0;
            var updatedItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException400("La cantidad debe ser mayor a 0");

                var dish = await _dishQuery.GetByIdAsync(item.Id);
                if (dish == null || !dish.Available)
                    throw new BadRequestException400("El plato especificado no está disponible");

                total += dish.Price * item.Quantity;

                updatedItems.Add(new OrderItem
                {
                    Dish = item.Id,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    Status = (int)OrderStatus.Pending, 
                    CreateDate = DateTime.UtcNow
                });

            }

            order.OrderItems = updatedItems;
            order.Price = total;
            order.UpdateDate = DateTime.UtcNow;

            var updated = await _command.UpdateAsync(order);

            return new OrderUpdateResponse
            {
                OrderNumber = updated.OrderId,
                TotalAmount = updated.Price,
                UpdateAt = updated.UpdateDate
            };
        }

    }
}
