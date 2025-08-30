using Application.Interfaces;
using Application.Models.Status;
using Application.Response.Status;
using Domain.Entities;

namespace Application.UseCases
{
    public class StatusService : IStatusService
    {
        private readonly IStatusCommand _command;
        private readonly IStatusQuery _query;

        public StatusService(IStatusCommand command, IStatusQuery query)
        {
            _command = command;
            _query = query;
        }

        public async Task<IReadOnlyList<StatusResponse>> GetAllAsync()
        {
            var statuses = await _query.GetAllAsync();
            return statuses.Select(status => new StatusResponse 
            { 
                Id = status.Id, 
                Name = status.Name 
            }).ToList();
        }

        public async Task<StatusResponse?> GetByIdAsync(int id)
        {
            var status = await _query.GetByIdAsync(id);
            if (status == null) return null;

            return new StatusResponse
            {
                Id = status.Id,
                Name = status.Name
            };
        }

        public async Task<StatusResponse> CreateAsync(CreateStatusRequest request)
        {
            var status = new Status
            {
                Name = request.Name
            };

            await _command.CreateAsync(status);
            return new StatusResponse { Id = status.Id, Name = status.Name };
        }

        public async Task<StatusResponse> UpdateAsync(int id, UpdateStatusRequest request)
        {
            var status = await _query.GetByIdAsync(id);
            if (status == null) throw new KeyNotFoundException("Status not found");

            status.Name = request.Name;
            await _command.UpdateAsync(status);

            return new StatusResponse { Id = status.Id, Name = status.Name };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _query.GetByIdAsync(id);
            if (status == null) return false;

            await _command.DeleteAsync(status);
            return true;
        }

    }
}
