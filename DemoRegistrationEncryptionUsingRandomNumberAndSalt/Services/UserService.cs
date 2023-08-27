using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Provider;
using SharedLogicLibrary.Models;
using SharedLogicLibrary.Models.Entities;
using System.Net.Http.Json;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public UserService( IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<UserSession> LoginUser(LoginModel loginModel)
        {
            var httpClient = httpClientFactory.CreateClient("MyApi");
            var user = await httpClient.PostAsJsonAsync("api/user/login", loginModel);
            var response = await user.Content.ReadFromJsonAsync<UserSession>();

            return response!;
        }

        public async Task<Response> RegisterUser(RegistrationEntity registrationEntity)
        {
            var httpClient = httpClientFactory.CreateClient("MyApi");
            var user = await httpClient.PostAsJsonAsync("api/user/register", registrationEntity);
            var response = await user.Content.ReadFromJsonAsync<Response>();
            return (response!);
        }
    }
}
