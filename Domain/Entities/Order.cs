namespace Domain.Entities
{
    public class Order
    {
        public long OrderId { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string DeliveryTo { get; set; }
        public Status OverallStatus { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
