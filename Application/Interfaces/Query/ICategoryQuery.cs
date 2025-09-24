using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface ICategoryQuery
    {
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int categoryId, CancellationToken cancellationToken = default);
    }
}
