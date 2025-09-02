using Application.Interfaces;
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

            return dishEntities.Select(dishEntity => new DishResponse
            {
                Id = dishEntity.DishId,
                Name = dishEntity.Name,
                Description = dishEntity.Description,
                Price = dishEntity.Price,
                IsActive = dishEntity.Available,
                Image = dishEntity.ImageUrl,
                CreatedAt = dishEntity.CreateDate,
                UpdatedAt = dishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    Id = dishEntity.Category.Id,
                    Name = dishEntity.Category.Name
                }
            }).ToList();
        }

        public async Task<DishResponse?> GetByIdAsync(Guid dishId)
        {
            var dishEntity = await _query.GetByIdAsync(dishId);
            if (dishEntity == null) return null;

            return new DishResponse
            {
                Id = dishEntity.DishId,
                Name = dishEntity.Name,
                Description = dishEntity.Description,
                Price = dishEntity.Price,
                IsActive = dishEntity.Available,
                Image = dishEntity.ImageUrl,
                CreatedAt = dishEntity.CreateDate,
                UpdatedAt = dishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    Id = dishEntity.Category.Id,
                    Name = dishEntity.Category.Name
                }
            };
        }

        public async Task<DishResponse> CreateAsync(DishRequest newDishRequest)
        {
            if (newDishRequest.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");
            
            if (await _query.ExistsByNameAsync(newDishRequest.Name))
                throw new InvalidOperationException("Ya existe un plato con ese nombre");

            var newDishEntity = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = newDishRequest.Name,
                Description = newDishRequest.Description,
                Price = newDishRequest.Price,
                Available = newDishRequest.IsActive,
                ImageUrl = newDishRequest.Image,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _command.CreateAsync(newDishEntity, newDishRequest.Category);

            return new DishResponse
            {
                Id = newDishEntity.DishId,
                Name = newDishEntity.Name,
                Description = newDishEntity.Description,
                Price = newDishEntity.Price,
                IsActive = newDishEntity.Available,
                Image = newDishEntity.ImageUrl,
                CreatedAt = newDishEntity.CreateDate,
                UpdatedAt = newDishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    Id = newDishEntity.Category.Id,
                    Name = newDishEntity.Category.Name
                }
            };
        }

        public async Task<DishResponse> UpdateAsync(Guid dishId, DishUpdateRequest updateDishRequest)
        {
            var dishEntity = await _query.GetByIdAsync(dishId);
            if (dishEntity == null) throw new KeyNotFoundException("Plato no encontrado");

            if (updateDishRequest.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            if (await _query.ExistsByNameAsync(updateDishRequest.Name))
                throw new InvalidOperationException("Ya existe un plato con ese nombre");

            dishEntity.Name = updateDishRequest.Name;
            dishEntity.Description = updateDishRequest.Description;
            dishEntity.Price = updateDishRequest.Price;
            dishEntity.Available = updateDishRequest.IsActive;
            dishEntity.ImageUrl = updateDishRequest.Image;
            dishEntity.UpdateDate = DateTime.UtcNow;

            await _command.UpdateAsync(dishEntity, updateDishRequest.Category);

            return new DishResponse
            {
                Id = dishEntity.DishId,
                Name = dishEntity.Name,
                Description = dishEntity.Description,
                Price = dishEntity.Price,
                IsActive = dishEntity.Available,
                Image = dishEntity.ImageUrl,
                CreatedAt = dishEntity.CreateDate,
                UpdatedAt = dishEntity.UpdateDate,
                Category = new CategoryResponse
                {
                    Id = dishEntity.Category.Id,
                    Name = dishEntity.Category.Name
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