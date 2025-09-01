using Application.Models;
using Application.Response;

namespace Application.Interfaces
{
    public interface IDishService
    {
        Task<IReadOnlyList<DishResponse>> GetAllAsync(string? name, int? categoryId, string? sortDirection);
        Task<DishResponse?> GetByIdAsync(Guid id);
        Task<DishResponse> CreateAsync(CreateDishRequest request);
        Task<DishResponse> UpdateAsync(Guid id, UpdateDishRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
