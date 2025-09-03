using Application.Models;
using Application.Response;

namespace Application.Interfaces
{
    public interface IDishService
    {
        Task<IReadOnlyList<DishResponse>> GetAllAsync(string? name, int? categoryId, string? sortByPrice, bool onlyActive);
        Task<DishResponse?> GetByIdAsync(Guid id);
        Task<DishResponse> CreateAsync(DishRequest request);
        Task<DishResponse> UpdateAsync(Guid id, DishUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
