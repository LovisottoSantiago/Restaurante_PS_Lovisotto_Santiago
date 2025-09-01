using Application.Interfaces;
using Application.Mappers;
using Application.Models;
using Application.Response;
using Domain.Entities;

namespace Application.UseCases
{
    public class DishService : IDishService
    {
        private readonly IDishCommand _command;
        private readonly IDishQuery _query;

        public DishService(IDishCommand dishCommand, IDishQuery dishQuery)
        {
            _command = dishCommand;
            _query = dishQuery;
        }

        public async Task<IReadOnlyList<DishResponse>> GetAllAsync(string? nameFilter, int? categoryFilter, string? sortDirection)
        {
            var dishEntities = await _query.GetAllAsync(nameFilter, categoryFilter, sortDirection);

            return dishEntities.Select(dishEntity => dishEntity.ToResponse()).ToList();
        }

        public async Task<DishResponse?> GetByIdAsync(Guid dishId)
        {
            var dishEntity = await _query.GetByIdAsync(dishId);
            if (dishEntity == null) return null;

            return new DishResponse
            {
                DishId = dishEntity.DishId,
                Name = dishEntity.Name,
                Description = dishEntity.Description,
                Price = dishEntity.Price,
                Available = dishEntity.Available,
                ImageUrl = dishEntity.ImageUrl,
                CreateDate = dishEntity.CreateDate,
                UpdateDate = dishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    CategoryId = dishEntity.Category.Id,
                    Name = dishEntity.Category.Name
                }
            };
        }

        public async Task<DishResponse> CreateAsync(CreateDishRequest newDishRequest)
        {
            var existingDishes = await _query.GetAllAsync(newDishRequest.Name, null, null);
            if (existingDishes.Any(d => d.Name == newDishRequest.Name))
                throw new InvalidOperationException("Ya existe un plato con ese nombre");

            var newDishEntity = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = newDishRequest.Name,
                Description = newDishRequest.Description,
                Price = newDishRequest.Price,
                Available = newDishRequest.Available,
                ImageUrl = newDishRequest.ImageUrl,
                CategoryId = newDishRequest.CategoryId,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _command.CreateAsync(newDishEntity);

            return new DishResponse
            {
                DishId = newDishEntity.DishId,
                Name = newDishEntity.Name,
                Description = newDishEntity.Description,
                Price = newDishEntity.Price,
                Available = newDishEntity.Available,
                ImageUrl = newDishEntity.ImageUrl,
                CreateDate = newDishEntity.CreateDate,
                UpdateDate = newDishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    CategoryId = newDishEntity.CategoryId,
                    Name = newDishEntity.Category?.Name ?? string.Empty
                }
            };
        }

        public async Task<DishResponse> UpdateAsync(Guid dishId, UpdateDishRequest updateDishRequest)
        {
            var dishEntity = await _query.GetByIdAsync(dishId);
            if (dishEntity == null) throw new KeyNotFoundException("Plato no encontrado");

            var conflictingDishes = await _query.GetAllAsync(updateDishRequest.Name, null, null);
            if (conflictingDishes.Any(d => d.Name == updateDishRequest.Name && d.DishId != dishId))
                throw new InvalidOperationException("Ya existe un plato con ese nombre");

            dishEntity.Name = updateDishRequest.Name;
            dishEntity.Description = updateDishRequest.Description;
            dishEntity.Price = updateDishRequest.Price;
            dishEntity.Available = updateDishRequest.Available;
            dishEntity.ImageUrl = updateDishRequest.ImageUrl;
            dishEntity.CategoryId = updateDishRequest.CategoryId;
            dishEntity.UpdateDate = DateTime.UtcNow;

            await _command.UpdateAsync(dishEntity);

            return new DishResponse
            {
                DishId = dishEntity.DishId,
                Name = dishEntity.Name,
                Description = dishEntity.Description,
                Price = dishEntity.Price,
                Available = dishEntity.Available,
                ImageUrl = dishEntity.ImageUrl,
                CreateDate = dishEntity.CreateDate,
                UpdateDate = dishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    CategoryId = dishEntity.CategoryId,
                    Name = dishEntity.Category?.Name ?? string.Empty
                }
            };
        }

        public async Task<bool> DeleteAsync(Guid dishId)
        {
            var dishEntity = await _query.GetByIdAsync(dishId);
            if (dishEntity == null) return false;

            await _command.DeleteAsync(dishEntity);
            return true;
        }
    }
}
