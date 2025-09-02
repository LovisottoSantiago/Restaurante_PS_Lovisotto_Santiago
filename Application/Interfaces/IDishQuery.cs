using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDishQuery
    {
        Task<IReadOnlyList<Dish>> GetAllAsync(string? nameFilter, int? categoryFilter, string? sortDirection);
        Task<Dish?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
