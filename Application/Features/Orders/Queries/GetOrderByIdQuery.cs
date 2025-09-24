using Application.Response;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderDetailsResponse>
    {
        public long Id { get; }

        public GetOrderByIdQuery(long id)
        {
            Id = id;
        }
    }
}
