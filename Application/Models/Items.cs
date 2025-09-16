using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class Items
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
