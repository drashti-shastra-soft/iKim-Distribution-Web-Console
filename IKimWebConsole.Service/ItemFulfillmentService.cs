using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class ItemFulfillmentService : IItemFulfillmentService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ItemFulfillmentService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null) 
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.ItemFulfillment> CreateAsync(Domain.ItemFulfillment mFulfillment)
        {
            return await _httpClientService.PostAsync<Domain.ItemFulfillment, Domain.ItemFulfillment>(
                url: $"{_baseUrl}/ItemFulfillment",
                obj: mFulfillment,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.ItemFulfillment> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.ItemFulfillment>(
            url: $"{_baseUrl}/ItemFulfillment/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }
    }
}
