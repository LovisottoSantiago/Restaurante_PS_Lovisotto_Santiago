using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Models;
using Application.Response;
using Domain.Enums;

namespace Application.UseCases.OrderUseCases
{
    public class UpdateOrderItemUseCase
    {
        private readonly IOrderQuery _query;
        private readonly IOrderItemCommand _itemCommand;
        private readonly IOrderCommand _orderCommand;

        public UpdateOrderItemUseCase(
            IOrderQuery query,
            IOrderItemCommand itemCommand,
            IOrderCommand orderCommand)
        {
            _query = query;
            _itemCommand = itemCommand;
            _orderCommand = orderCommand;
        }

        public async Task<OrderUpdateResponse> ExecuteAsync(long orderId, long itemId, OrderItemUpdateRequest request)
        {           
            var order = await _query.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException404("La orden no existe");

            if (order.OverallStatus == (int)OrderStatus.Closed)
                throw new BadRequestException400("No se puede modificar una orden cerrada");

            var updatedItem = await _itemCommand.UpdateStatusAsync(orderId, itemId, request.Status);
            if (updatedItem == null)
                throw new NotFoundException404("El item no existe en la orden");

            order = await _query.GetByIdAsync(orderId);

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
            
            await _orderCommand.UpdateStatusAsync(order.OrderId, order.OverallStatus, order.UpdateDate);

            return new OrderUpdateResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                UpdateAt = order.UpdateDate
            };
        }
    }
}
