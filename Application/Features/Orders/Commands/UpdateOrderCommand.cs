using Application.Models;
using Application.Response;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<OrderUpdateResponse>
    {
        public long OrderId { get; }
        public OrderUpdateRequest Request { get; }
        public UpdateOrderCommand(long orderId, OrderUpdateRequest request)
        {
            OrderId = orderId;
            Request = request;
        }
    }
}
