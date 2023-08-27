using System.ComponentModel.DataAnnotations;

namespace SharedLogicLibrary.Models
{
    public class LoginModel
    {
        [EmailAddress, Required]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
