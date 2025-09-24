using Application.Models;
using Application.Response;
using MediatR;

namespace Application.UseCases.DishUseCases.UpdateDish
{   
    public class UpdateDishCommand : IRequest<DishResponse>
    {
        public Guid Id { get; }
        public DishUpdateRequest Request { get; }

        public UpdateDishCommand(Guid id, DishUpdateRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}

