namespace Application.Models
{
    public class CreateDishRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
    }
}
