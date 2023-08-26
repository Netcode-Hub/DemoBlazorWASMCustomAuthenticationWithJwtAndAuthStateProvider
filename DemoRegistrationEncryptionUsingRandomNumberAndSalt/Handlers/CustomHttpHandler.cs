using Blazored.LocalStorage;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Handlers
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorageService;

        public CustomHttpHandler(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri!.AbsolutePath.ToLower().Contains("login") || request.RequestUri!.AbsolutePath.ToLower().Contains("register"))
                return await base.SendAsync(request, cancellationToken);

            var token = await localStorageService.GetItemAsync<string>("JwtToken", cancellationToken = default);
            if (token is not null)
                request.Headers.Add("Authorization", $"Bearer {token}");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
