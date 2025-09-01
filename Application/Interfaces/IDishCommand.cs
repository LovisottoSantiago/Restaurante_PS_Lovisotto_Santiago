using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDishCommand
    {
        Task CreateAsync(Dish dish);
        Task UpdateAsync(Dish dish);
        Task DeleteAsync(Dish dish);
    }
}
