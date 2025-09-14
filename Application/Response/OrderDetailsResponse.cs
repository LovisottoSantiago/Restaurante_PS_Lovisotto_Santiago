using Application.Models;

namespace Application.Response
{
    public class OrderDetailsResponse
    {
        public long OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryTo { get; set; }
        public string Notes { get; set; }
        public GenericResponse Status {  get; set; }
        public GenericResponse DeliveryType { get; set; }
        public List<OrderItemResponse> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
