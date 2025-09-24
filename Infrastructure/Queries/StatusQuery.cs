using Application.Interfaces.Query;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class StatusQuery : IStatusQuery
    {
        private readonly AppDbContext _context;

        public StatusQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Status>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Statuses
                .AsNoTracking()
                .ToListAsync(cancellationToken); 
        }
    }
}
