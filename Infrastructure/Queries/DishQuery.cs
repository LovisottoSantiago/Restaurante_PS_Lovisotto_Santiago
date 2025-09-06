using Application.Interfaces.Query;
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

        public async Task<IReadOnlyList<Dish>> GetAllAsync()
        {
            return await _context.Dishes
                .Include(dish => dish.CategoryNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Dish?> GetByIdAsync(Guid id)
        {
            return await _context.Dishes
                .Include(dish => dish.CategoryNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(dish => dish.DishId == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Dishes
                .AnyAsync(dish => dish.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid excludeId)
        {
            return await _context.Dishes
                .AnyAsync(dish => dish.Name == name && dish.DishId != excludeId);
        }

    }
}
