using Application.Interfaces.Command;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands
{
    public class OrderItemCommand : IOrderItemCommand
    {
        private readonly AppDbContext _context;

        public OrderItemCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> UpdateStatusAsync(long orderId, long itemId, int newStatus)
        {
            var item = await _context.OrderItems
                .Include(oi => oi.OrderNavigation)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == itemId && oi.Order == orderId);

            if (item == null)
                return null; // el UseCase lanza NotFound

            item.Status = newStatus;
            item.OrderNavigation.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return item;
        }
    }
}
