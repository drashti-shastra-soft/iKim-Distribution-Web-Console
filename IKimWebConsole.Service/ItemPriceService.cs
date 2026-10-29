using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class ItemPriceService : IItemPriceService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ItemPriceService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<List<Domain.ItemPrice>> GetByAsync(int id)
        {
            return await _httpClientService.GetAsync<List<Domain.ItemPrice>>(
                url: $"{_baseUrl}/ItemPrice/ItemId/{id}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
