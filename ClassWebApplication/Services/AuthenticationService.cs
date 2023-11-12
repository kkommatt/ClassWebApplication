using Microsoft.Identity.Client;

namespace ClassWebApplication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = _configuration["Authentication:ClientId"];
            var clientSecret = _configuration["Authentication:ClientSecret"];
            var authority = _configuration["Authentication:Authority"];
            var scopes = _configuration["Authentication:Scopes"].Split(',');

            var cca = ConfidentialClientApplicationBuilder
                        .Create(clientId)
                        .WithClientSecret(clientSecret)
                        .WithAuthority(new Uri(authority))
                        .Build();

            var result = await cca.AcquireTokenForClient(scopes)
                                .ExecuteAsync();

            return result.AccessToken;
        }
    }
}
