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

        public async Task<IReadOnlyList<Dish>> GetAllAsync()
        {
            return await _context.Dishes
                .Include(d => d.CategoryNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Dish?> GetByIdAsync(Guid id)
        {
            return await _context.Dishes
                .Include(d => d.CategoryNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DishId == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Dishes
                .AnyAsync(d => d.Name == name);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
