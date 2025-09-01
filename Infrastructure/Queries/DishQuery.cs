using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDbContext _context;

        public DishQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Dish>> GetAllAsync(string? nameFilter, int? categoryFilter, string? sortDirection)
        {
            var query = _context.Dishes
                .Include(d => d.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
                query = query.Where(d => d.Name.Contains(nameFilter));

            if (categoryFilter.HasValue)
                query = query.Where(d => d.CategoryId == categoryFilter.Value);

            if (!string.IsNullOrEmpty(sortDirection))
            {
                if (sortDirection.ToUpper() == "ASC")
                    query = query.OrderBy(d => d.Price);
                else if (sortDirection.ToUpper() == "DESC")
                    query = query.OrderByDescending(d => d.Price);
            }

            return await query.ToListAsync();
        }

        public async Task<Dish?> GetByIdAsync(Guid id)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DishId == id);
        }
    }
}
