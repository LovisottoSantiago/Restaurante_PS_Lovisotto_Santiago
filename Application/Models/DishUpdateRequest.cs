using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class DishUpdateRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Category { get; set; }
        public string? Image { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
