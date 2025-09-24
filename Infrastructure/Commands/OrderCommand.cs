using Application.Interfaces.Command;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands
{
    public class OrderCommand : IOrderCommand
    {
        private readonly AppDbContext _context;

        public OrderCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> InsertAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task UpdateStatusAsync(long orderId, int overallStatus, DateTime updateDate, CancellationToken cancellationToken = default)
        {
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null) return;

                order.OverallStatus = overallStatus;
                order.UpdateDate = updateDate;

                await _context.SaveChangesAsync();
            }
        }
    }
}
