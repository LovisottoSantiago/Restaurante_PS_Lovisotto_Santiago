using Application.Models.Status;
using Application.Response.Status;

namespace Application.Interfaces
{
    public interface IStatusService
    {
        Task<IReadOnlyList<StatusResponse>> GetAllAsync();
        Task<StatusResponse?> GetByIdAsync(int id);
        Task<StatusResponse> CreateAsync(CreateStatusRequest request);
        Task<StatusResponse> UpdateAsync(int id, UpdateStatusRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
