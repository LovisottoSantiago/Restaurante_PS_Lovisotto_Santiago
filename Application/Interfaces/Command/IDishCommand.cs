using Domain.Entities;

namespace Application.Interfaces.Command
{
    public interface IDishCommand
    {
        Task InsertAsync(Dish dish, CancellationToken cancellationToken = default);
        Task UpdateAsync(Dish dish, CancellationToken cancellationToken = default);
        Task DeleteAsync(Dish dish, CancellationToken cancellationToken = default);
    }
}
