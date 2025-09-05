using Application.Models;
using Application.Response;

namespace Application.Interfaces.Service
{
    public interface IDishService
    {
        Task<IReadOnlyList<DishResponse>> GetAllAsync(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive);
        Task<DishResponse?> GetByIdAsync(Guid id);
        Task<DishResponse> CreateAsync(DishRequest request);
        Task<DishResponse> UpdateAsync(Guid id, DishUpdateRequest request);
    }
}
