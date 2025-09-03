using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class DishUpdateRequest
    {
        [Required(ErrorMessage = "El nombre del plato es obligatorio")]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Price { get; set; }

        [Required]
        public int Category { get; set; }

        [Url]
        public string? Image { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
