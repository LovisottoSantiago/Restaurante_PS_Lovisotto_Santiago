using Application.Interfaces.Query;
using Application.Response;
using MediatR;

namespace Application.Features.DeliveryTypes.Queries
{
    public class GetAllDeliveryTypesHandler : IRequestHandler<GetAllDeliveryTypesQuery, IReadOnlyList<GenericResponse>>
    {
        private readonly IDeliveryTypeQuery _query;

        public GetAllDeliveryTypesHandler(IDeliveryTypeQuery query)
        {
            _query = query;
        }
        public async Task<IReadOnlyList<GenericResponse>> Handle(GetAllDeliveryTypesQuery request, CancellationToken cancellationToken)
        {
            var deliveryTypes = await _query.GetAllAsync(cancellationToken);

            return deliveryTypes.Select(deliveryType =>
            new GenericResponse
            {
                Id = deliveryType.Id,
                Name = deliveryType.Name
            }).ToList();
        }
    }
}
