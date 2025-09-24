using Application.Response;
using MediatR;

namespace Application.Features.DeliveryTypes.Queries
{
    public class GetAllDeliveryTypesQuery : IRequest<IReadOnlyList<GenericResponse>>
    {
    }
}
