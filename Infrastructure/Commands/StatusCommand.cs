using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands
{
    public class StatusCommand : IStatusCommand
    {
        private readonly AppDbContext _context;

        public StatusCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Status status)
        {
            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Status status)
        {
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
        }

    }
}
