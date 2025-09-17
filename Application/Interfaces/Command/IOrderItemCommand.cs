using Domain.Entities;

namespace Application.Interfaces.Command
{
    public interface IOrderItemCommand
    {
        Task<OrderItem> UpdateStatusAsync(long orderId, long itemId, int newStatus);
    }
}
