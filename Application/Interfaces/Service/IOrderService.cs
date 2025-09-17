using Application.Models;
using Application.Response;

namespace Application.Interfaces.Service
{
    public interface IOrderService
    {
        Task<IReadOnlyList<OrderDetailsResponse>> GetAllAsync(DateTime? from, DateTime? to, int? status);
        Task<OrderDetailsResponse?> GetByIdAsync(long id);
        Task<OrderCreateResponse> CreateAsync(OrderRequest request);
        Task<OrderUpdateResponse> UpdateAsync(long id, OrderUpdateRequest request);
        Task<OrderUpdateResponse> UpdateItemAsync(long orderId, long itemId, OrderItemUpdateRequest request);
    }
}
