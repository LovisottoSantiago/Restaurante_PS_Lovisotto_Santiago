using Application.Response;

namespace Application.Interfaces.Service
{
    public interface IStatusService
    {
        Task<IReadOnlyList<GenericResponse>> GetAllAsync();
    }
}
