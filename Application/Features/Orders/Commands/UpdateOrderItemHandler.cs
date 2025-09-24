using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class UpdateOrderItemHandler : IRequestHandler<UpdateOrderItemCommand, OrderUpdateResponse>
    {
        private readonly IOrderQuery _query;
        private readonly IOrderItemCommand _itemCommand;
        private readonly IOrderCommand _orderCommand;

        public UpdateOrderItemHandler(IOrderQuery query, IOrderItemCommand itemCommand, IOrderCommand orderCommand)
        {
            _query = query;
            _itemCommand = itemCommand;
            _orderCommand = orderCommand;
        }

        public async Task<OrderUpdateResponse> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var order = await _query.GetByIdAsync(command.OrderId, cancellationToken);

            if (order == null)
                throw new NotFoundException404("Orden no encontrada");

            if (order.OverallStatus == (int)OrderStatus.Closed)
                throw new BadRequestException400("No se puede modificar una orden cerrada");

            var updatedItem = await _itemCommand.UpdateStatusAsync(command.OrderId, command.ItemId, request.Status, cancellationToken);
            if (updatedItem == null)
                throw new NotFoundException404("Item no encontrado en la orden");

            order = await _query.GetByIdAsync(command.OrderId, cancellationToken);

            var statuses = order.OrderItems.Select(i => i.Status).ToList();
            if (statuses.All(s => s == (int)OrderStatus.Delivery))
                order.OverallStatus = (int)OrderStatus.Delivery;
            else if (statuses.All(s => s == (int)OrderStatus.Ready))
                order.OverallStatus = (int)OrderStatus.Ready;
            else if (statuses.All(s => s == (int)OrderStatus.InProgress))
                order.OverallStatus = (int)OrderStatus.InProgress;
            else if (statuses.All(s => s == (int)OrderStatus.Pending))
                order.OverallStatus = (int)OrderStatus.Pending;
            else
                order.OverallStatus = (int)OrderStatus.InProgress;

            order.UpdateDate = DateTime.UtcNow;

            await _orderCommand.UpdateStatusAsync(order.OrderId, order.OverallStatus, order.UpdateDate, cancellationToken);

            return new OrderUpdateResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                UpdateAt = order.UpdateDate
            };
        }
    }
}
