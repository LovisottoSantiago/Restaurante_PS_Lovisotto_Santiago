using Application.Response;

namespace Application.Interfaces.Service
{
    public interface IDeliveryTypeService
    {
        Task<IReadOnlyList<GenericResponse>> GetAllAsync();
    }
}
