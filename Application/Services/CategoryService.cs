using Application.Interfaces.Service;
using Application.Response;
using Application.UseCases.CategoryUseCases;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly GetAllCategoriesUseCase _getAll;

        public CategoryService(GetAllCategoriesUseCase getAll)
        {
            _getAll = getAll;
        }

        public Task<IReadOnlyList<CategoryResponse>> GetAllAsync()
        {
            return _getAll.ExecuteAsync();
        }
    }
}
