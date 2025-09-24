using Application.Interfaces.Command;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands
{
    public class DishCommand : IDishCommand
    {
        private readonly AppDbContext _context;

        public DishCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(Dish dish, CancellationToken cancellationToken = default)
        {
            await _context.Dishes.AddAsync(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dish dish, CancellationToken cancellationToken = default)
        {
            _context.Entry(dish).Property(d => d.Category).IsModified = true;
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Dish dish, CancellationToken cancellationToken = default)
        {
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }
    }
}
