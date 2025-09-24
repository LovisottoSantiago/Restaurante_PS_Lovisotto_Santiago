using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IDeliveryTypeQuery
    {
        Task<IReadOnlyList<DeliveryType>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
