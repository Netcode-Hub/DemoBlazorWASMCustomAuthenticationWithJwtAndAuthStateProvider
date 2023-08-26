using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Models;
using System.Net.Http.Json;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public UserService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<Response> LoginUser(LoginModel loginModel)
        {
            var httpClient = httpClientFactory.CreateClient("MyApi");
            var user = await httpClient.PostAsJsonAsync("api/user/login", loginModel);
            var response = await user.Content.ReadFromJsonAsync<Response>();
            return (response!);
        }

        public async Task<Response> RegisterUser(RegistrationModel registrationModel)
        {
            var httpClient = httpClientFactory.CreateClient("MyApi");
            var user = await httpClient.PostAsJsonAsync("api/user", registrationModel);
            var response = await user.Content.ReadFromJsonAsync<Response>();
            return (response!);
        }
    }
}
