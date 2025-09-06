using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Application.UseCases.DishUseCases;

namespace Application.Services
{
    public class DishService : IDishService
    {
        private readonly GetAllDishesUseCase _getAll;
        private readonly GetDishByIdUseCase _getById;
        private readonly CreateDishUseCase _create;
        private readonly UpdateDishUseCase _update;

        public DishService(GetAllDishesUseCase getAll, GetDishByIdUseCase getById, CreateDishUseCase create, UpdateDishUseCase update)
        {
            _getAll = getAll;
            _getById = getById;
            _create = create;
            _update = update;
        }

        public Task<IReadOnlyList<DishResponse>> GetAllAsync(string? name, int? categoryId, SortDirection? sortByPrice, bool onlyActive)
        {
            return _getAll.ExecuteAsync(name, categoryId, sortByPrice, onlyActive);
        }

        public Task<DishResponse?> GetByIdAsync(Guid id)
        {
            return _getById.ExecuteAsync(id);
        }

        public Task<DishResponse> CreateAsync(DishRequest request)
        {
            return _create.ExecuteAsync(request);
        }

        public Task<DishResponse> UpdateAsync(Guid id, DishUpdateRequest request)
        {
            return _update.ExecuteAsync(id, request);
        }
    }
}
