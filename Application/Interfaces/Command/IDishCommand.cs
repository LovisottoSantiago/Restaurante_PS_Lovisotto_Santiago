using Domain.Entities;

namespace Application.Interfaces.Command
{
    public interface IDishCommand
    {
        Task InsertAsync(Dish dish);
        Task UpdateAsync(Dish dish);
        Task SoftDeleteAsync(Dish dish);
    }
}
