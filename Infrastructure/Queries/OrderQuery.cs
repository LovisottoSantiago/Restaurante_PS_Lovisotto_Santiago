using Application.Interfaces.Query;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class OrderQuery : IOrderQuery
    {
        private readonly AppDbContext _context;

        public OrderQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(order => order.DeliveryTypeNavigation)
                .Include(order => order.OverallStatusNavigation)
                .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.DishNavigation)
                .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.StatusNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(long orderId)
        {
            return await _context.Orders
                .Include(order => order.DeliveryTypeNavigation)
                .Include(order => order.OverallStatusNavigation)
                .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.DishNavigation)
                .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.StatusNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(order => order.OrderId == orderId);
        }
        public async Task<bool> ExistsActiveOrderWithDishAsync(Guid dishId)
        {
            return await _context.OrderItems
                .AnyAsync(oi => oi.Dish == dishId &&
                                oi.OrderNavigation.OverallStatus != 3 && // Cancelada
                                oi.OrderNavigation.OverallStatus != 4); // Finalizada
        }

    }
}
