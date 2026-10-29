using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public DashboardService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<List<Domain.Data>> SalesChartAsync(int storeId)
        {
            var result = await _httpClientService.PostAsync<List<Domain.Data>>(
                url: $"{_baseUrl}/Chart/SalesChart/{storeId}",
                obj: null,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<List<Domain.Data>> PurchaseChartAsync(int vendorId)
        {
            var result = await _httpClientService.PostAsync<List<Domain.Data>>(
                    url: $"{_baseUrl}/Chart/PurchaseChart/{vendorId}",
                    obj: null,
                    token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<List<Domain.Data>> IngredientConsumptionReportAsync(int itemId)
        {
            var result = await _httpClientService.PostAsync<List<Domain.Data>>(
                    url: $"{_baseUrl}/Chart/IngredientConsumptionReport/{itemId}",
                    obj: null,
                    token: _token
            ).ConfigureAwait(false);

            return result;
        }

    }
}
