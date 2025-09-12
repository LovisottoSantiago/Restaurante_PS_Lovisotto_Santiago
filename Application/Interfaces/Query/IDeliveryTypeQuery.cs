using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IDeliveryTypeQuery
    {
        Task<IReadOnlyList<DeliveryType>> GetAllAsync();
    }
}
