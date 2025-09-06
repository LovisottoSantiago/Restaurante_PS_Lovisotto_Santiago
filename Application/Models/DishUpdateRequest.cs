using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class DishUpdateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } 
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int Category { get; set; }

        [Url]
        public string? Image { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
