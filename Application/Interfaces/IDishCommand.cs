using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDishCommand
    {
        Task CreateAsync(Dish dish, int categoryId);
        Task UpdateAsync(Dish dish, int categoryId);
        Task DeleteAsync(Dish dish);
    }
}
