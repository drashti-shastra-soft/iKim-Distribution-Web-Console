using IKimWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;

namespace IKimWebConsole.Service
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;

        public ChangePasswordService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
        }

        public async Task Initiate(Domain.ChangePassword mChangePassword)
        {
            await _httpClientService.PutAsync<Domain.ChangePassword>(
                url: $"{_baseUrl}/ChangePassword",
                obj: mChangePassword,
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
