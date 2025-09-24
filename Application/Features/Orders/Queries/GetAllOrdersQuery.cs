using Application.Response;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDetailsResponse>>
    {
        public DateTime? From { get; }
        public DateTime? To { get; }
        public int? Status { get; }

        public GetAllOrdersQuery(DateTime? from, DateTime? to, int? status)
        {
            From = from;
            To = to;
            Status = status;
        }
    }
}
