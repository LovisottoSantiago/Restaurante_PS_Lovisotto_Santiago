using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IOrderQuery
    {
        Task<IReadOnlyList<Order>> GetAllAsync(DateTime? from, DateTime? to, int? status, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(long orderId, CancellationToken cancellationToken = default);
        Task<bool> ExistsOrderWithDishAsync(Guid dishId, CancellationToken cancellationToken = default);
    }
}
