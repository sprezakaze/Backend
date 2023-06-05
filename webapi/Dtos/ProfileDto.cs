using System.ComponentModel.DataAnnotations;
using webapi.Entities;

namespace webapi.Dtos
{
    public class ProfileDto
    {
        [Required]
        public User User { get; set; } = null!;
        [Required]
        public List<CartItem> Cart { get; set; } = null!;
    }
}
