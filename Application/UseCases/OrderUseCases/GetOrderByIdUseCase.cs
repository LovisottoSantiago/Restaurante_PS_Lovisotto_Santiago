using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.OrderUseCases
{
    public class GetOrderByIdUseCase
    {
        private readonly IOrderQuery _query;

        public GetOrderByIdUseCase(IOrderQuery orderQuery)
        {
            _query = orderQuery;
        }

        public async Task<OrderDetailsResponse?> ExecuteAsync(long id)
        {
            var order = await _query.GetByIdAsync(id);

            if (order == null)
                throw new NotFoundException404("Orden no encontrada");

            return new OrderDetailsResponse
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
            };
        }
    }
}
