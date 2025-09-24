using Application.Interfaces.Query;
using Application.Models;
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

        public async Task<IReadOnlyList<Dish>> GetAllAsync(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive, CancellationToken cancellationToken = default)
        {
            var query = _context.Dishes
                .Include(dish => dish.CategoryNavigation)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.Name.Contains(name));

            if (onlyActive)
                query = query.Where(d => d.Available);

            if (categoryId.HasValue)
                query = query.Where(d => d.Category == categoryId.Value);

            if (sortByPrice.HasValue)
            {
                query = sortByPrice == SortDirection.asc
                    ? query.OrderBy(d => d.Price)
                    : query.OrderByDescending(d => d.Price);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Dish?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Dishes
                .Include(dish => dish.CategoryNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(dish => dish.DishId == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Dishes
                .AnyAsync(dish => dish.Name == name, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default)
        {
            return await _context.Dishes
                .AnyAsync(dish => dish.Name == name && dish.DishId != excludeId, cancellationToken);
        }

    }
}
