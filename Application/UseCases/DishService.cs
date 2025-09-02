using Application.Interfaces;
using Application.Models;
using Application.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.UseCases
{
    public class DishService : IDishService
    {
        private readonly IDishQuery _dishQuery;
        private readonly IDishCommand _dishCommand;

        public DishService(IDishQuery dishQuery, IDishCommand dishCommand)
        {
            _dishQuery = dishQuery;
            _dishCommand = dishCommand;
        }

        public async Task<IReadOnlyList<DishResponse>> GetAllAsync(string? name, int? categoryId, string? sortDirection)
        {
            var dishes = await _dishQuery.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(name))
                dishes = dishes.Where(d => d.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (categoryId.HasValue)
                dishes = dishes.Where(d => d.Category == categoryId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(sortDirection))
            {
                dishes = sortDirection.ToUpper() switch
                {
                    "ASC" => dishes.OrderBy(d => d.Price).ToList(),
                    "DESC" => dishes.OrderByDescending(d => d.Price).ToList(),
                    _ => dishes
                };
            }

            return dishes.Select(d => new DishResponse
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Category = new CategoryResponse
                {
                    Id = d.CategoryNavigation.Id,
                    Name = d.CategoryNavigation.Name
                },
                IsActive = d.Available,
                Image = d.ImageUrl,
                CreatedAt = d.CreateDate,
                UpdatedAt = d.UpdateDate
            }).ToList();
        }

        public async Task<DishResponse?> GetByIdAsync(Guid id)
        {
            var d = await _dishQuery.GetByIdAsync(id);
            if (d is null) return null;

            return new DishResponse
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Category = new CategoryResponse
                {
                    Id = d.CategoryNavigation.Id,
                    Name = d.CategoryNavigation.Name
                },
                IsActive = d.Available,
                Image = d.ImageUrl,
                CreatedAt = d.CreateDate,
                UpdatedAt = d.UpdateDate
            };
        }

        public async Task<DishResponse> CreateAsync(DishRequest request)
        {
            if (await _dishQuery.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException($"Plato con el nombre '{request.Name}' ya existe.");

            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                Price = request.Price,
                Category = request.Category,
                ImageUrl = request.Image ?? string.Empty,
                Available = request.IsActive,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _dishCommand.CreateAsync(dish);

            // Lo traigo de nuevo con la categoría ya incluida
            var dishCreated = await _dishQuery.GetByIdAsync(dish.DishId);

            return new DishResponse
            {
                Id = dishCreated.DishId,
                Name = dishCreated.Name,
                Description = dishCreated.Description,
                Price = dishCreated.Price,
                Category = new CategoryResponse
                {
                    Id = dishCreated.CategoryNavigation.Id,
                    Name = dishCreated.CategoryNavigation.Name
                },
                IsActive = dishCreated.Available,
                Image = dishCreated.ImageUrl,
                CreatedAt = dishCreated.CreateDate,
                UpdatedAt = dishCreated.UpdateDate
            };
        }

        public async Task<DishResponse> UpdateAsync(Guid id, DishUpdateRequest request)
        {
            var dish = await _dishQuery.GetByIdAsync(id);
            if (dish is null)
                throw new KeyNotFoundException($"Plato con el id '{id}' no existe.");

            dish.Name = request.Name;
            dish.Description = request.Description ?? string.Empty;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.ImageUrl = request.Image ?? string.Empty;
            dish.Available = request.IsActive;
            dish.UpdateDate = DateTime.UtcNow;

            await _dishCommand.UpdateAsync(dish);

            // Lo traigo de nuevo con la categoría ya incluida
            var updated = await _dishQuery.GetByIdAsync(dish.DishId);

            return new DishResponse
            {
                Id = updated.DishId,
                Name = updated.Name,
                Description = updated.Description,
                Price = updated.Price,
                Category = new CategoryResponse
                {
                    Id = updated.CategoryNavigation.Id,
                    Name = updated.CategoryNavigation.Name
                },
                IsActive = updated.Available,
                Image = updated.ImageUrl,
                CreatedAt = updated.CreateDate,
                UpdatedAt = updated.UpdateDate
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var dish = await _dishQuery.GetByIdAsync(id);
            if (dish is null)
                return false;

            await _dishCommand.DeleteAsync(dish);
            return true;
        }
    }
}
