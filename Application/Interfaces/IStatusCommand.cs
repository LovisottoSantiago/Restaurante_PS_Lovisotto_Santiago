using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStatusCommand
    {
        Task CreateAsync(Status status);
        Task UpdateAsync(Status status);
        Task DeleteAsync(Status status);
    }
}
