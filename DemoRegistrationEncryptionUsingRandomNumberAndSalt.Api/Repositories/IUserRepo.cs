using SharedLogicLibrary.Models;
using SharedLogicLibrary.Models.Entities;
namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Repositories
{
    public interface IUserRepo
    {
        Task<Response> RegisterUserAsync(RegistrationEntity registrationEntity);
        Task<UserSession> LoginUserAsync(LoginModel loginModel);
    }
}
