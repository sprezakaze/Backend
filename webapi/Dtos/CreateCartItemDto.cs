using System.ComponentModel.DataAnnotations;
using webapi.Entities;

namespace webapi.Dtos
{
    public class CreateCartItemDto
    {
        [Required]
        public int ClothingId { get; set; }
        [Required, Range(0, 100)]
        public int Count { get; set; }
    }
}
