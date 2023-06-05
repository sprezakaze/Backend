using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
