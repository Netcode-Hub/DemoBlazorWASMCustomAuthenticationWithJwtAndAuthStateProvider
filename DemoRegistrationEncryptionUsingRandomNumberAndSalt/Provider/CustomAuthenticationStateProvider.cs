using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharedLogicLibrary.Models;
using System.Security.Claims;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Provider
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        // inject and initialize localstorage service
        private readonly ILocalStorageService localStorageService;
        private readonly ICryptoService cryptoService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, ICryptoService cryptoService)
        {
            this.localStorageService = localStorageService;
            this.cryptoService = cryptoService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var encryptedUserSession = await localStorageService.GetItemAsync<UserSession>("UserData");
            if (encryptedUserSession is null)
                return await Task.FromResult(new AuthenticationState(_anonymous));


            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
            new Claim(ClaimTypes.Name,  await cryptoService.DecryptAsync(encryptedUserSession.Username!.ToString())),
            new Claim(ClaimTypes.Role, await cryptoService.DecryptAsync(encryptedUserSession.Role!.ToString()))
            }, "JwtAuth"));

            //Call the Utility class and pass in the token to decrypt
            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        public void NotifyAuthenticationState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
