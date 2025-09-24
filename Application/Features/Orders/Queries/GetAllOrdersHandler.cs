using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IReadOnlyList<OrderDetailsResponse>>
    {
        private readonly IOrderQuery _query;

        public GetAllOrdersHandler(IOrderQuery orderQuery)
        {
            _query = orderQuery;
        }

        public async Task<IReadOnlyList<OrderDetailsResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            if (request.From.HasValue && request.To.HasValue && request.From > request.To)
                throw new BadRequestException400("Rango de fechas inválido");

            var orders = await _query.GetAllAsync(request.From, request.To, request.Status, cancellationToken);

            var response = orders.Select(order => new OrderDetailsResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                DeliveryTo = order.DeliveryTo,
                Notes = order.Notes,
                Status = new GenericResponse
                {
                    Id = order.OverallStatusNavigation.Id,
                    Name = order.OverallStatusNavigation.Name
                },
                DeliveryType = new GenericResponse
                {
                    Id = order.DeliveryTypeNavigation.Id,
                    Name = order.DeliveryTypeNavigation.Name
                },
                Items = order.OrderItems.Select(item => new OrderItemResponse
                {
                    Id = item.OrderItemId,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    Status = new GenericResponse
                    {
                        Id = item.StatusNavigation.Id,
                        Name = item.StatusNavigation.Name
                    },
                    Dish = new DishShortResponse
                    {
                        Id = item.DishNavigation.DishId,
                        Name = item.DishNavigation.Name,
                        Image = item.DishNavigation.ImageUrl
                    }
                }).ToList(),
                CreatedAt = order.CreateDate,
                UpdatedAt = order.UpdateDate
            }).ToList();

            return response;
        }
    }
}
