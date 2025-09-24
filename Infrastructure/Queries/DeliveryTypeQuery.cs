using Application.Interfaces.Query;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class DeliveryTypeQuery : IDeliveryTypeQuery
    {
        private readonly AppDbContext _context;

        public DeliveryTypeQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<DeliveryType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.DeliveryTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int deliveryTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.DeliveryTypes.AnyAsync(d => d.Id == deliveryTypeId);
        }
    }
}
