using Domain.Entities;

namespace Application.Interfaces.Command
{
    public interface IOrderCommand
    {
        Task<Order> InsertAsync(Order order);
    }
}
