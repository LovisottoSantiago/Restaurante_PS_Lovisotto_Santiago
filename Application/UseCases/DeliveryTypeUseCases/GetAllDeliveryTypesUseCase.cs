using Application.Interfaces.Query;
using Application.Response;

namespace Application.UseCases.DeliveryTypeUseCases
{
    public class GetAllDeliveryTypesUseCase
    {
        private readonly IDeliveryTypeQuery _query;

        public GetAllDeliveryTypesUseCase(IDeliveryTypeQuery query)
        {
            _query = query;
        }

        public async Task<IReadOnlyList<GenericResponse>> ExecuteAsync()
        {
            var deliveryTypes = await _query.GetAllAsync();

            return deliveryTypes.Select(deliveryType =>
            new GenericResponse
            {
                Id = deliveryType.Id,
                Name = deliveryType.Name
            }).ToList();
        }

    }
}
