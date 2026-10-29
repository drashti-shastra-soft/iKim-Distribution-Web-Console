using IKimWebConsole.Infrastructure.IService;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiBaseUrl;
        public AuthenticationService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _apiBaseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");
        }

        public async Task<Domain.User> AuthenticateAsync(Domain.Credential mCredential)
        {
            return await _httpClientService.PostAsync<Domain.Credential, Domain.User>(
                url: $"{_apiBaseUrl}/Authentication",
                obj: mCredential
            );
        }
    }
}
