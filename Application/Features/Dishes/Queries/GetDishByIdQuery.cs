using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Queries
{
    public class GetDishByIdQuery : IRequest<DishResponse>
    {
        public Guid Id { get; }
        public GetDishByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
