using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class DishRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Category { get; set; }

        [Url]
        public string? Image { get; set; }
    }
}
