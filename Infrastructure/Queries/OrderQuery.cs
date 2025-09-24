using Application.Interfaces.Query;
using Domain.Entities;
using Domain.Enums;
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

        public async Task<IReadOnlyList<Order>> GetAllAsync(DateTime? from, DateTime? to, int? status, CancellationToken cancellationToken = default)
        {
            var query = _context.Orders
                .Include(order => order.DeliveryTypeNavigation)
                .Include(order => order.OverallStatusNavigation)
                .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.DishNavigation)
                .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.StatusNavigation)
                .AsNoTracking()
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(o => o.CreateDate >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreateDate <= to.Value);

            if (status.HasValue)
                query = query.Where(o => o.OverallStatus == status.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(long orderId, CancellationToken cancellationToken = default)
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
        public async Task<bool> ExistsOrderWithDishAsync(Guid dishId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AnyAsync(o => o.OrderItems.Any(oi => oi.Dish == dishId));
        }

    }
}
