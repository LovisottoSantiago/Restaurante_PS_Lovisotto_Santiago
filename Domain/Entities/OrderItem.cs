namespace Domain.Entities
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public Order Order { get; set; }
        public long OrderId { get; set; }
        public Dish Dish { get; set; }
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public Status Status { get; set; }
        public int StatusId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
