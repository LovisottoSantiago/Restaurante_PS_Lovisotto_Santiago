
using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.CategoryUseCases
{
    public class GetAllCategoriesUseCase
    {
        private readonly ICategoryQuery _query;

        public GetAllCategoriesUseCase(ICategoryQuery query)
        {
            _query = query;
        }

        public async Task<IReadOnlyList<CategoryResponse>> ExecuteAsync()
        {
            var categories = await _query.GetAllAsync();

            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Order = category.Order
            }).ToList();
        }
    }
}
