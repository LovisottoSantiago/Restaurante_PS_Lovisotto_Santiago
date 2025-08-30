using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStatusQuery
    {
        Task<IReadOnlyList<Status>> GetAllAsync();
        Task<Status?> GetByIdAsync(int id);
    }
}
