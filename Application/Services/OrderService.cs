using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Application.UseCases.OrderUseCases;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly GetAllOrdersUseCase _getAll;
        private readonly CreateOrderUseCase _create;
        private readonly GetOrderByIdUseCase _getById;

        public OrderService(GetAllOrdersUseCase getAll, GetOrderByIdUseCase getById, CreateOrderUseCase create)
        {
            _getAll = getAll;
            _getById = getById;
            _create = create;
        }

        public Task<IReadOnlyList<OrderDetailsResponse>> GetAllAsync(DateTime? from, DateTime? to, int? status)
        {
            return _getAll.ExecuteAsync(from, to, status);
        }

        public Task<OrderDetailsResponse?> GetByIdAsync(long id)
        {
            return _getById.ExecuteAsync(id);
        }
        public Task<OrderCreateResponse> CreateAsync(OrderRequest request)
        {
            return _create.ExecuteAsync(request);
        }

    }
}
