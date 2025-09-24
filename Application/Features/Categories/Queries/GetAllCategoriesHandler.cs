using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryResponse>>
    {
        private readonly ICategoryQuery _query;

        public GetAllCategoriesHandler(ICategoryQuery query)
        {
            _query = query;
        }

        public async Task<IReadOnlyList<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _query.GetAllAsync(cancellationToken);

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
