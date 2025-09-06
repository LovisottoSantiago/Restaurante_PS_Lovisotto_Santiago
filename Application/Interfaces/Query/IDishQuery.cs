using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IDishQuery
    {
        Task<IReadOnlyList<Dish>> GetAllAsync();
        Task<Dish?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name, Guid excludeId);
    }
}
