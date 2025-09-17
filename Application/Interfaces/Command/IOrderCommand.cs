using Domain.Entities;

namespace Application.Interfaces.Command
{
    public interface IOrderCommand
    {
        Task<Order> InsertAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task UpdateStatusAsync(long orderId, int overallStatus, DateTime updateDate);
    }
}
