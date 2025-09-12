using Application.Response;

namespace Application.Interfaces.Service
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryResponse>> GetAllAsync();
    }
}
