using Application.Models;
using Application.Response;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class CreateOrderCommand : IRequest<OrderCreateResponse>
    {
        public OrderRequest Request { get; }
        public CreateOrderCommand(OrderRequest request)
        {
            Request = request;
        }
    }
}
