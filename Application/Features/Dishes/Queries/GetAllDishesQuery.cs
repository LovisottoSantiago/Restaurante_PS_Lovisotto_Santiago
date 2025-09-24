using Application.Models;
using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Queries
{
    public class GetAllDishesQuery : IRequest<IReadOnlyList<DishResponse>>
    {
        public string? Name { get; }
        public int? CategoryId { get; }
        public SortDirection? SortByPrice { get; }
        public bool OnlyActive { get; }
        public GetAllDishesQuery(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive)
        {
            Name = name;
            CategoryId = categoryId;
            SortByPrice = sortByPrice;
            OnlyActive = onlyActive;
        }
    }
}
