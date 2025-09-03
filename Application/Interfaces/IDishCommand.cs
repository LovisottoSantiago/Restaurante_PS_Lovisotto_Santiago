using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDishCommand
    {
        Task InsertAsync(Dish dish);
        Task UpdateAsync(Dish dish);
        Task DeleteAsync(Dish dish);
    }
}
