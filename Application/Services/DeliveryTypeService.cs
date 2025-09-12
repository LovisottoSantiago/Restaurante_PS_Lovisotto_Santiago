using Application.Interfaces.Service;
using Application.Response;
using Application.UseCases.DeliveryTypeUseCases;

namespace Application.Services
{
    public class DeliveryTypeService : IDeliveryTypeService
    {
        private readonly GetAllDeliveryTypesUseCase _getAll;

        public DeliveryTypeService(GetAllDeliveryTypesUseCase getAll)
        {
            _getAll = getAll;
        }

        public Task<IReadOnlyList<GenericResponse>> GetAllAsync()
        {
            return _getAll.ExecuteAsync();
        }
    }
}
