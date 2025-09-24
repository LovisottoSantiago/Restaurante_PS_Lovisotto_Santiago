using Application.Models;
using Application.Response;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class UpdateOrderItemCommand : IRequest<OrderUpdateResponse>
    {
        public long OrderId { get; }
        public long ItemId { get; }
        public OrderItemUpdateRequest Request { get; }
        public UpdateOrderItemCommand(long orderId, long itemId, OrderItemUpdateRequest request)
        {
            OrderId = orderId;
            ItemId = itemId;
            Request = request;
        }
    }
}
