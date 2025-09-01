using Application.Response;
using Domain.Entities;

namespace Application.Mappers
{
    public static class DishMapper
    {
        public static DishResponse ToResponse(this Dish dish)
        {
            return new DishResponse
            {
                DishId = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Available = dish.Available,
                ImageUrl = dish.ImageUrl,
                CreateDate = dish.CreateDate,
                UpdateDate = dish.UpdateDate,
                Category = new CategoryResponse
                {
                    CategoryId = dish.Category.Id,
                    Name = dish.Category.Name
                }
            };
        }
    }
}
