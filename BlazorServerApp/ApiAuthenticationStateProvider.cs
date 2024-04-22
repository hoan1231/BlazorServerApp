using BlazorServerApp;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorServerApp
{
    public class ApiAuthenticationStateProvider: AuthenticationStateProvider
    {

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "mrfibuli"),
        }, "Custom Authentication");

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
