using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class DishUpdateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Category { get; set; }

        [Url]
        public string? Image { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
