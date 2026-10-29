using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;

namespace IKimWebConsole.Service
{
    public class DailyOrderService : IDailyOrderService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public DailyOrderService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<List<Domain.DailyOrder>> DailyOrdersAsync(Domain.DailyOrder mDailyOrders)
        {
            var result = await _httpClientService.PostAsync<Domain.DailyOrder, List<Domain.DailyOrder>>(
                url: $"{_baseUrl}/DailyOrder/DailyOrders",
                obj: mDailyOrders,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<string> GetPdfAsync(List<Domain.DailyOrder> mDailyOrder)
        {
            return await _httpClientService.PostAsync<List<Domain.DailyOrder>, string>(
               url: $"{_baseUrl}/DailyOrder/GenerateDailyOrderChallan",
               obj: mDailyOrder,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<List<Domain.DailyOrder>>GetSelectedDailyOrdersAsync(List<int> mDailyOrder)
        {
            var result = await _httpClientService.PostAsync<List<int>,List<Domain.DailyOrder>>
            (
                url:$"{_baseUrl}/DailyOrder/GetSelectedDailyOrders",
                obj: mDailyOrder,
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

    }
}
