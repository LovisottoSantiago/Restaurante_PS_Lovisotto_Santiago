using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.OrderUseCases
{
    public class GetAllOrdersUseCase
    {
        private readonly IOrderQuery _query;

        public GetAllOrdersUseCase(IOrderQuery orderQuery)
        {
            _query = orderQuery;
        }

        public async Task<IReadOnlyList<OrderDetailsResponse>> ExecuteAsync(DateTime? from, DateTime? to, int? status)
        {
            var orders = await _query.GetAllAsync();

            if (from.HasValue && to.HasValue && from > to)
                throw new BadRequestException400("Rango de fechas inválido");

            // Filtros
            if (from.HasValue)
                orders = orders.Where(o => o.CreateDate >= from.Value).ToList();

            if (to.HasValue)
                orders = orders.Where(o => o.CreateDate <= to.Value).ToList();

            if (status.HasValue)
                orders = orders.Where(o => o.OverallStatus == status.Value).ToList();

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
