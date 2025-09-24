using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class UpdateOrderItemHandler : IRequestHandler<UpdateOrderItemCommand, OrderUpdateReponse>
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

        public async Task<OrderUpdateReponse> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var order = await _query.GetByIdAsync(command.OrderId, cancellationToken);

            if (order == null)
                throw new NotFoundException404("Orden no encontrada");

            if (order.OverallStatus == (int)OrderStatus.Closed)
                throw new BadRequestException400("No se puede modificar una orden cerrada");

            if (order.OverallStatus == (int)OrderStatus.Ready)
                throw new BadRequestException400("No se puede modificar una orden que ya está lista para entregar");

            if (order.OverallStatus == (int)OrderStatus.Delivery)
                throw new BadRequestException400("No se puede modificar una orden que ya está en proceso de entrega");

            var updatedItem = await _itemCommand.UpdateStatusAsync(command.OrderId, command.ItemId, request.Status, cancellationToken);

            if (updatedItem == null)
                throw new NotFoundException404("Item no encontrado en la orden");

            order = await _query.GetByIdAsync(command.OrderId, cancellationToken);

            var statuses = order.OrderItems.Select(i => i.Status).ToList();
            var overallStatus = statuses.Min();
            order.OverallStatus = overallStatus;

            order.UpdateDate = DateTime.UtcNow;

            await _orderCommand.UpdateStatusAsync(order.OrderId, order.OverallStatus, order.UpdateDate, cancellationToken);

            return new OrderUpdateReponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                UpdateAt = order.UpdateDate
            };
        }
    }
}
