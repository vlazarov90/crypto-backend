using System.ComponentModel.DataAnnotations;

namespace CryptoDashboard.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}