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

            if (!Enum.IsDefined(typeof(OrderStatus), request.Status))
                throw new BadRequestException400("El estado especificado no es válido");

            var item = order.OrderItems.FirstOrDefault(i => i.OrderItemId == command.ItemId);
            if (item == null)
                throw new NotFoundException404("Item no encontrado en la orden");
            
            if (request.Status <= item.Status)
                throw new BadRequestException400("No se puede cambiar de 'Entregado' a 'En preparación'");
            
            var updatedItem = await _itemCommand.UpdateStatusAsync(command.OrderId, command.ItemId, request.Status, cancellationToken);
            if (updatedItem == null)
                throw new NotFoundException404("Error al actualizar el item");
            
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
