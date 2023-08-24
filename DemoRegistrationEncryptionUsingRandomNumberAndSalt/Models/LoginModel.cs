using System.ComponentModel.DataAnnotations;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Models
{
    public class LoginModel
    {
        [EmailAddress, Required]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
