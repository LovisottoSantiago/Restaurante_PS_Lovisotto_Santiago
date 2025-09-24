using Application.Response;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryResponse>>
    {
    }
}
