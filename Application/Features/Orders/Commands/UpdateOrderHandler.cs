using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderUpdateResponse>
    {
        private readonly IOrderCommand _command;
        private readonly IOrderQuery _orderQuery;
        private readonly IDishQuery _dishQuery;

        public UpdateOrderHandler(IOrderCommand command, IOrderQuery query, IDishQuery dishQuery)
        {
            _command = command;
            _orderQuery = query;
            _dishQuery = dishQuery;
        }

        public async Task<OrderUpdateResponse> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var order = await _orderQuery.GetByIdAsync(command.OrderId, cancellationToken);

            if (order == null)
                throw new NotFoundException404("Orden no encontrada");

            if (order.OverallStatus == (int)OrderStatus.Closed)
                throw new BadRequestException400("No se puede modificar una orden cerrada");

            decimal total = 0;
            var updatedItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException400("La cantidad debe ser mayor a 0");

                var dish = await _dishQuery.GetByIdAsync(item.Id, cancellationToken);
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

            var updated = await _command.UpdateAsync(order, cancellationToken);

            return new OrderUpdateResponse
            {
                OrderNumber = updated.OrderId,
                TotalAmount = updated.Price,
                UpdateAt = updated.UpdateDate
            };
        }
    }
}
