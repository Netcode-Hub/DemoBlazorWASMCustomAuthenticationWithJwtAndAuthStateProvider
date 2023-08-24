namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models
{
    public class RegistrationModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

