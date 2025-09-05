using Application.Interfaces.Query;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class CategoryQuery : ICategoryQuery
    {
        private readonly AppDbContext _context;

        public CategoryQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }

}
