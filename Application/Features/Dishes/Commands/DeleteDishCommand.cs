using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Commands
{     
    public class DeleteDishCommand : IRequest<DishResponse>
    {
        public Guid Id { get; }
        public DeleteDishCommand(Guid id) 
        { 
            Id = id;
        }
    }
}
