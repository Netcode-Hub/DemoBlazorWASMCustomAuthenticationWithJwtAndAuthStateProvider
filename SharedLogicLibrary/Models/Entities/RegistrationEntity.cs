using System.ComponentModel.DataAnnotations;

namespace SharedLogicLibrary.Models.Entities
{
    public class RegistrationEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

