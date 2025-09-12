using Application.Interfaces.Service;
using Application.Response;
using Application.UseCases.StatusUseCases;

namespace Application.Services
{
    public class StatusService : IStatusService
    {
        private readonly GetAllStatusesUseCase _getAll;

        public StatusService(GetAllStatusesUseCase getAll)
        {
            _getAll = getAll;
        }

        public Task<IReadOnlyList<GenericResponse>> GetAllAsync()
        {
            return _getAll.ExecuteAsync();
        }
    }
}
