using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public OrderService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.OrderLister> GetAllAsync(Domain.OrderLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.OrderLister>(
                 url: $"{_baseUrl}/Order/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Order> CreateAsync(Domain.Order mOrder)
        {
            return await _httpClientService.PostAsync<Domain.Order>(
               url: $"{_baseUrl}/Order",
               obj: mOrder,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.Order> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Order>(
                url: $"{_baseUrl}/Order/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Order/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<string> GetPdfAsync(List<Domain.Order> mOrder)
        {
            return await _httpClientService.PostAsync<List<Domain.Order>, string>(
               url: $"{_baseUrl}/Order/GenerateDeliveryChallan",
               obj: mOrder,
               token: _token
           ).ConfigureAwait(false);
        }
    }
}
