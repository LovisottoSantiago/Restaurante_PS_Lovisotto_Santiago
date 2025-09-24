using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IStatusQuery
    {
        Task<IReadOnlyList<Status>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
