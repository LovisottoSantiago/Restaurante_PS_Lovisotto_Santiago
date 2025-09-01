namespace Application.Response
{
    public class DishResponse
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Relación con categoría
        public CategoryResponse Category { get; set; }
    }
}
