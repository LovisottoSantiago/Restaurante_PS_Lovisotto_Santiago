using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.StatusUseCases
{
    public class GetAllStatusesUseCase
    {
        private readonly IStatusQuery _query;

        public GetAllStatusesUseCase(IStatusQuery query)
        {
            _query = query;
        }

        public async Task<IReadOnlyList<GenericResponse>> ExecuteAsync()
        {
            var statuses = await _query.GetAllAsync();

            return statuses.Select(status =>
            new GenericResponse
            {
                Id = status.Id,
                Name = status.Name
            }).ToList();
        }

    }
}
