using IKimWebConsole.Infrastructure.IService;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class DailySalesService : IDailySalesService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public DailySalesService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.DailySalesLister> GetAllAsync(Domain.DailySalesLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.DailySalesLister>(
                 url: $"{_baseUrl}/DailySales/GetAll",
                 obj: mLister,
                 token: _token
            ).ConfigureAwait(false);

            return result;
        }
    }
}
