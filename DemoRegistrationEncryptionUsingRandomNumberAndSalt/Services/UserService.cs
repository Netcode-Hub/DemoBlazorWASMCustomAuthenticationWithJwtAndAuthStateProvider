using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Models;
using System.Net.Http.Json;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response> LoginUser(LoginModel loginModel)
        {
            var user = await httpClient.PostAsJsonAsync("api/user/login", loginModel);
            var response = await user.Content.ReadFromJsonAsync<Response>();
            return (response!);
        }

        public async Task<Response> RegisterUser(RegistrationModel registrationModel)
        {
            var user = await httpClient.PostAsJsonAsync("api/user", registrationModel);
            var response = await user.Content.ReadFromJsonAsync<Response>();
            return (response!);
        }
    }
}
