using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IDishQuery
    {
        Task<IReadOnlyList<Dish>> GetAllAsync(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive, CancellationToken cancellationToken = default);
        Task<Dish?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default);
    }
}
