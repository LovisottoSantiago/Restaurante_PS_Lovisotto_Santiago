using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IOrderQuery
    {
        Task<IReadOnlyList<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(long orderId);
        Task<bool> ExistsActiveOrderWithDishAsync(Guid dishId);
    }
}
