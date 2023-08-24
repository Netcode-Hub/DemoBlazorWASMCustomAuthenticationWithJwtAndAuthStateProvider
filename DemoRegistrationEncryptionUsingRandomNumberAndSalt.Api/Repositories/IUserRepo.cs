using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories
{
    public interface IUserRepo
    {
        Task<Response> RegisterUserAsync(RegistrationModel registrationModel);
        Task<Response> LoginUserAsync(LoginModel loginModel);
    }
}
