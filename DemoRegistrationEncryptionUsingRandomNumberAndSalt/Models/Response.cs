namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? AccessToken { get; set; }
    }
}
