using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.Features.Statuses.Queries
{
    public class GetAllStatusesHandler : IRequestHandler<GetAllStatusesQuery, IReadOnlyList<GenericResponse>>
    {
        private readonly IStatusQuery _query;

        public GetAllStatusesHandler(IStatusQuery query)
        {
            _query = query;
        }
        public async Task<IReadOnlyList<GenericResponse>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = await _query.GetAllAsync(cancellationToken);

            return statuses.Select(status =>
            new GenericResponse
            {
                Id = status.Id,
                Name = status.Name
            }).ToList();
        }
    }
}
