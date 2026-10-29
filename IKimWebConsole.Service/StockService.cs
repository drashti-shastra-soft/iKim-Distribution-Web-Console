using IKimWebConsole.Infrastructure.IService;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class StockService : IStockService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public StockService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.StockLister> GetAllAsync(Domain.StockLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.StockLister>(
                 url: $"{_baseUrl}/Stock/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Stock> CreateAsync(Domain.Stock mStock)
        {
            return await _httpClientService.PostAsync<Domain.Stock>(
               url: $"{_baseUrl}/Stock",
               obj: mStock,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.Stock> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Stock>(
                url: $"{_baseUrl}/Stock/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Stock/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
