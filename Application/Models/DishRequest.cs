using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class DishRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required]
        public int Category { get; set; }

        [Url]
        public string? Image { get; set; }

        public bool IsActive { get; set; }
    }
}
