using Application.Models;
using Application.Response;
using MediatR;

namespace Application.Features.Dishes.Commands
{
    public class CreateDishCommand : IRequest<DishResponse>
    {
        public DishRequest Request { get; }
        public CreateDishCommand(DishRequest request)
        {
            Request = request;
        }
    }

}


