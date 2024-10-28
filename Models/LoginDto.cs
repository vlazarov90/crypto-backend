using System.ComponentModel.DataAnnotations;

namespace CryptoDashboard.Api.Models
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}