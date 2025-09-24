using Domain.Entities;

namespace Application.Interfaces.Command
{
    public interface IOrderCommand
    {
        Task<Order> InsertAsync(Order order, CancellationToken cancellationToken = default);
        Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(long orderId, int overallStatus, DateTime updateDate, CancellationToken cancellationToken = default);
    }
}
