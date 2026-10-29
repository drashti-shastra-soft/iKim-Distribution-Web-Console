using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class OrderLineService : IOrderLineService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public OrderLineService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.OrderLineLister> GetAllAsync(Domain.OrderLineLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.OrderLineLister>(
                 url: $"{_baseUrl}/OrderLine/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.OrderLine> CreateAsync(Domain.OrderLine mOrderLine)
        {
            return await _httpClientService.PostAsync<Domain.OrderLine>(
               url: $"{_baseUrl}/OrderLine",
               obj: mOrderLine,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<List<Domain.OrderLine>> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<List<Domain.OrderLine>>(
                url: $"{_baseUrl}/OrderLine/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.OrderLine> GetByAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.OrderLine>(
                         url: $"{_baseUrl}/OrderLine/GetOrderLine/{id}",
                         token: _token
                         ).ConfigureAwait(false);
             return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/OrderLine/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<decimal> GetPriceAsync(int orderId, int itemId)
        {
            var result = await _httpClientService.GetAsync<decimal>(
                url: $"{_baseUrl}/ItemPrice/GetPrice?orderId={orderId}&itemId={itemId}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<List<Domain.OrderLine>> GetByOrderId(int orderId)
        {
            var result = await _httpClientService.GetAsync<List<Domain.OrderLine>>(
               url: $"{_baseUrl}/OrderLine/GetByOrder/{orderId}",
               token: _token
            ).ConfigureAwait(false);

            return result;
        }


    }
}
