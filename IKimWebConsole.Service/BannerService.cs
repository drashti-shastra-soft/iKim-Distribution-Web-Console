using IKimWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;

namespace IKimWebConsole.Service
{
    public class BannerService : IBannerService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public BannerService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.BannerLister> GetAllAsync(Domain.BannerLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.BannerLister>(
                url: $"{_baseUrl}/Banner/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task<Domain.Banner> CreateAsync(Domain.Banner mBanner)
        {
            var result = await _httpClientService.PostAsync<Domain.Banner>(
               url: $"{_baseUrl}/Banner",
               obj: mBanner,
               token: _token
               ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Banner/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
