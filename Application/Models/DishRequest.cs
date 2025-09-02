namespace Application.Models
{
    public class DishRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Category { get; set; }
        public bool IsActive { get; set; }
        public string? Image { get; set; }
    }
}
