using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface ICategoryQuery
    {
        Task<IReadOnlyList<Category>> GetAllAsync();
        Task<bool> ExistsAsync(int categoryId);
    }
}
