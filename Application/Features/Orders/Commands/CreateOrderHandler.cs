using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Response;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderCreateReponse>
    {
        private readonly IOrderCommand _command;
        private readonly IDishQuery _dishQuery;
        private readonly IDeliveryTypeQuery _deliveryQuery;

        public CreateOrderHandler(IOrderCommand command, IDishQuery dishQuery, IDeliveryTypeQuery deliveryQuery)
        {
            _command = command;
            _dishQuery = dishQuery;
            _deliveryQuery = deliveryQuery;
        }

        public async Task<OrderCreateReponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            if (request.Delivery == null || request.Delivery.Id <= 0)
                throw new BadRequestException400("Debe especificar un tipo de entrega válido");

            if (!await _deliveryQuery.ExistsAsync(request.Delivery.Id, cancellationToken))
                throw new BadRequestException400("Debe especificar un tipo de entrega válido");

            if (request.Items == null || request.Items.Count == 0)
                throw new BadRequestException400("Debe especificar al menos un plato");

            decimal total = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException400("La cantidad debe ser mayor a 0");

                var dish = await _dishQuery.GetByIdAsync(item.Id, cancellationToken);
                if (dish == null || !dish.Available)
                    throw new BadRequestException400("El plato especificado no existe o no está disponible");

                total += dish.Price * item.Quantity;

                orderItems.Add(new OrderItem
                {
                    Dish = item.Id,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    Status = (int)OrderStatus.Pending,
                    CreateDate = DateTime.UtcNow
                });
            }

            var order = new Order
            {
                DeliveryType = request.Delivery.Id,
                DeliveryTo = request.Delivery.To,
                Notes = request.Notes,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                OverallStatus = (int)OrderStatus.Pending, 
                Price = total,
                OrderItems = orderItems
            };

            var orderCreated = await _command.InsertAsync(order, cancellationToken);

            return new OrderCreateReponse
            {
                OrderNumber = orderCreated.OrderId,
                TotalAmount = orderCreated.Price,
                CreatedAt = orderCreated.CreateDate
            };
        }
    }
}
