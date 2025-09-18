using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class OrderItemUpdateRequest
    {
        [Required]
        public int Status { get; set; }
    }
}
