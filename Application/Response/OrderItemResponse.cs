namespace Application.Response
{
    public class OrderItemResponse
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public GenericResponse Status { get; set; }
        public DishShortResponse Dish { get; set; }
    }
}
