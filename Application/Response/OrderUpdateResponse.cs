namespace Application.Response
{
    public class OrderUpdateResponse
    {
        public long OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
