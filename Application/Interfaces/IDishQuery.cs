using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDishQuery
    {
        Task<IReadOnlyList<Dish>> GetAllAsync();
        Task<Dish?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
