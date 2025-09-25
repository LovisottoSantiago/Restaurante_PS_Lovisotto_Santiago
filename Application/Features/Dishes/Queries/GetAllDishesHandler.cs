using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Models;
using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Queries
{
    public class GetAllDishesHandler : IRequestHandler<GetAllDishesQuery, IReadOnlyList<DishResponse>>    
    {
        private readonly IDishQuery _query;

        public GetAllDishesHandler(IDishQuery query)
        {
            _query = query;
        }

        public async Task<IReadOnlyList<DishResponse>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
        {
            var dishes = await _query.GetAllAsync(request.Name, request.CategoryId, request.SortByPrice, request.OnlyActive, cancellationToken);

            return dishes.Select(d => new DishResponse
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Category = new GenericResponse
                {
                    Id = d.CategoryNavigation.Id,
                    Name = d.CategoryNavigation.Name
                },
                IsActive = d.Available,
                Image = d.ImageUrl,
                CreatedAt = d.CreateDate,
                UpdatedAt = d.UpdateDate
            }).ToList();
        }
    }
}
