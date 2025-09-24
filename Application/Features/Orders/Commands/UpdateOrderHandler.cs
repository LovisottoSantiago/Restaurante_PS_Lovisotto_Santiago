using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderUpdateReponse>
    {
        private readonly IOrderCommand _command;
        private readonly IOrderQuery _orderQuery;
        private readonly IDishQuery _dishQuery;

        public UpdateOrderHandler(IOrderCommand command, IOrderQuery query, IDishQuery dishQuery)
        {
            _command = command;
            _orderQuery = query;
            _dishQuery = dishQuery;
        }

        public async Task<OrderUpdateReponse> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var order = await _orderQuery.GetByIdAsync(command.OrderId, cancellationToken);

            if (order == null)
                throw new NotFoundException404("Orden no encontrada");

            if (request.Items == null || !request.Items.Any())
                throw new BadRequestException400("Debe especificar al menos un item para actualizar");

            if (order.OverallStatus == (int)OrderStatus.Closed)
                throw new BadRequestException400("No se puede modificar una orden cerrada");

            if (order.OverallStatus != (int)OrderStatus.Pending)
                throw new BadRequestException400("No se puede modificar una orden que ya está en preparación");

            decimal total = 0;

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException400("La cantidad debe ser mayor a 0");

                var dish = await _dishQuery.GetByIdAsync(item.Id, cancellationToken);
                if (dish == null || !dish.Available)
                    throw new BadRequestException400("El plato especificado no está disponible");

                total += dish.Price * item.Quantity;

                var existingItem = order.OrderItems.FirstOrDefault(i => i.Dish == item.Id);

                if (existingItem != null)
                {
                    if (existingItem.Status != (int)OrderStatus.Pending)
                        throw new BadRequestException400("No se puede modificar un item que ya está en preparación");

                    existingItem.Quantity = item.Quantity;
                    existingItem.Notes = item.Notes;
                    existingItem.CreateDate = DateTime.UtcNow;
                }
                else
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        Dish = item.Id,
                        Quantity = item.Quantity,
                        Notes = item.Notes,
                        Status = (int)OrderStatus.Pending,
                        CreateDate = DateTime.UtcNow,
                        DishNavigation = dish 
                    });
                }
            }
            
            order.Price = order.OrderItems.Sum(i => i.Quantity * i.DishNavigation.Price);

            order.UpdateDate = DateTime.UtcNow;

            if (order.OrderItems.Any())
                order.OverallStatus = order.OrderItems.Min(i => i.Status);

            var updated = await _command.UpdateAsync(order, cancellationToken);

            return new OrderUpdateReponse
            {
                OrderNumber = updated.OrderId,
                TotalAmount = updated.Price,
                UpdateAt = updated.UpdateDate
            };
        }
    }
}
