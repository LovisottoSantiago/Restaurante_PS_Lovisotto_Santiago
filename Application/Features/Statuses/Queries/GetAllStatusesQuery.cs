using Application.Response;
using MediatR;

namespace Application.Features.Statuses.Queries
{
    public class GetAllStatusesQuery : IRequest<IReadOnlyList<GenericResponse>>
    {
    }
}
