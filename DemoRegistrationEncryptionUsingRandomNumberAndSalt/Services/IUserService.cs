using SharedLogicLibrary.Models;
using SharedLogicLibrary.Models.Entities;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Services
{
    public interface IUserService
    {
        Task<Response> RegisterUser(RegistrationEntity registrationEntity);
        Task<UserSession> LoginUser(LoginModel loginModel);

    }
}
