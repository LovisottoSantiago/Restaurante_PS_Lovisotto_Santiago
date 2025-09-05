using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Domain.Entities;
namespace Application.UseCases
{
    public class DishService : IDishService
    {
        private readonly IDishQuery _query;
        private readonly IDishCommand _command;
        private readonly ICategoryQuery _categoryQuery; 

        public DishService(IDishQuery dishQuery, IDishCommand dishCommand, ICategoryQuery categoryQuery)
        {
            _query = dishQuery;
            _command = dishCommand;
            _categoryQuery = categoryQuery;
        }

        public async Task<IReadOnlyList<DishResponse>> GetAllAsync(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive)
        {
            var dishes = await _query.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(name))
                dishes = dishes.Where(dish => dish.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (onlyActive)
                dishes = dishes.Where(dish => dish.Available).ToList();

            if (categoryId.HasValue)
                dishes = dishes.Where(dish => dish.Category == categoryId.Value).ToList();

            if (sortByPrice.HasValue)
            {
                if (sortByPrice == SortDirection.asc)
                    dishes = dishes.OrderBy(d => d.Price).ToList();
                else if (sortByPrice == SortDirection.desc)
                    dishes = dishes.OrderByDescending(d => d.Price).ToList();
                else
                    throw new ArgumentException("Parámetros de ordenamiento inválidos");
            }

            return dishes.Select(d => new DishResponse
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Category = new GenericResponse
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
            var dish = await _query.GetByIdAsync(id);
            if (dish == null) return null;

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = dish.CategoryNavigation.Id,
                    Name = dish.CategoryNavigation.Name
                },
                IsActive = dish.Available,
                Image = dish.ImageUrl,
                CreatedAt = dish.CreateDate,
                UpdatedAt = dish.UpdateDate
            };
        }

        public async Task<DishResponse> CreateAsync(DishRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentNullException("El nombre del plato es obligatorio");

            if (request.Price <= 0)
                throw new ArgumentOutOfRangeException("El precio debe ser mayor a cero");
            
            if (!await _categoryQuery.ExistsAsync(request.Category))
                throw new ArgumentException("La categoría debe existir");

            if (await _query.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException("Ya existe un plato con ese nombre");

            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                ImageUrl = request.Image,
                Available = true,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _command.InsertAsync(dish);

            // Lo traigo de nuevo con la categoría ya incluida
            var dishCreated = await _query.GetByIdAsync(dish.DishId);

            return new DishResponse
            {
                Id = dishCreated.DishId,
                Name = dishCreated.Name,
                Description = dishCreated.Description,
                Price = dishCreated.Price,
                Category = new GenericResponse
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
            var dish = await _query.GetByIdAsync(id);
            if (dish is null)
                throw new KeyNotFoundException("Plato no encontrado");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentNullException("El nombre del plato es obligatorio");

            if (request.Price <= 0)
                throw new ArgumentOutOfRangeException("El precio debe ser mayor a cero");

            if (!await _categoryQuery.ExistsAsync(request.Category))
                throw new ArgumentException("La categoría debe existir");

            if (await _query.ExistsByNameAsync(request.Name) && !dish.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Ya existe un plato con ese nombre");

            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.ImageUrl = request.Image;
            dish.Available = request.IsActive;
            dish.UpdateDate = DateTime.UtcNow;

            await _command.UpdateAsync(dish);
            
            var updated = await _query.GetByIdAsync(dish.DishId);

            return new DishResponse
            {
                Id = updated.DishId,
                Name = updated.Name,
                Description = updated.Description,
                Price = updated.Price,
                Category = new GenericResponse
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

    }
}
