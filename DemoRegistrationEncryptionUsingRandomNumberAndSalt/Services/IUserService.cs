using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Models;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Services
{
    public interface IUserService
    {
        Task<Response> RegisterUser(RegistrationModel registrationModel);
        Task<Response> LoginUser(LoginModel loginModel);

    }
}
