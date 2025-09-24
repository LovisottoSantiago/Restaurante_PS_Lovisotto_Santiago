using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class Delivery
    {
        [Required]
        public int Id { get; set; }   
        public string? To { get; set; }
    }
}
