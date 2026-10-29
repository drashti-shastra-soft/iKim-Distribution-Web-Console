using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class PurchaseItemService : IPurchaseItemService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PurchaseItemService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.PurchaseItemLister> GetAllAsync(Domain.PurchaseItemLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PurchaseItemLister>(
                      url: $"{_baseUrl}/PurchaseItem/GetAll",
                      obj: mLister,
                      token: _token
              ).ConfigureAwait(false);

            return result;
        }

        public async Task<List<Domain.PurchaseItem>> GetByAsync(int id)
        {
            return await _httpClientService.GetAsync<List<Domain.PurchaseItem>>(
               url: $"{_baseUrl}/PurchaseItem/{id}",
               token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.PurchaseItem> CreateAsync(Domain.PurchaseItem mPurchaseItem)
        {
            return await _httpClientService.PostAsync<Domain.PurchaseItem>(
                url: $"{_baseUrl}/PurchaseItem",
                obj: mPurchaseItem,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.PurchaseItem> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.PurchaseItem>(
                url: $"{_baseUrl}/PurchaseItem/Purchase/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }


        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/PurchaseItem/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<decimal> GetPriceAsync(int purchaseId, int itemId)
        {
            var result = await _httpClientService.GetAsync<decimal>(
                url: $"{_baseUrl}/ItemPrice/GetPrice?orderId={purchaseId}&itemId={itemId}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }
    }
}
