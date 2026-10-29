using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;

namespace IKimWebConsole.Service
{
    public class StockUpdateService : IStockUpdateService
    { 
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public StockUpdateService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.StockUpdate> CreateAsync(Domain.StockUpdate mStockUpdate)
        {
            return await _httpClientService.PostAsync<Domain.StockUpdate>(
               url: $"{_baseUrl}/StockUpdate",
               obj: mStockUpdate,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<List<Domain.StockUpdate>> GetByItemId(int itemId)
        {
            var result = await _httpClientService.GetAsync<List<Domain.StockUpdate>>(
               url: $"{_baseUrl}/StockUpdate/GetByItem/{itemId}",
               token: _token
            ).ConfigureAwait(false);
            return result;
        }



    }
}
