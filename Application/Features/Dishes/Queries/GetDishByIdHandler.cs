using Application.Exceptions;
using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Queries
{
    public class GetDishByIdHandler : IRequestHandler<GetDishByIdQuery, DishResponse>
    {
        private readonly IDishQuery _query;

        public GetDishByIdHandler(IDishQuery query)
        {
            _query = query;
        }

        public async Task<DishResponse> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new BadRequestException400("Formato de ID inválido");

            var dish = await _query.GetByIdAsync(request.Id, cancellationToken);

            if (dish == null)
                throw new NotFoundException404("Plato no encontrado");

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = dish.CategoryNavigation.Id,
                    Name = dish.CategoryNavigation.Name
                },
                IsActive = dish.Available,
                Image = dish.ImageUrl,
                CreatedAt = dish.CreateDate,
                UpdatedAt = dish.UpdateDate
            };
        }
    }
}
