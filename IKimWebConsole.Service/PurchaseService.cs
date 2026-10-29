using IKimWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;
using System;

namespace IKimWebConsole.Service
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PurchaseService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.PurchaseLister> GetAllAsync(Domain.PurchaseLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PurchaseLister>(
                url: $"{_baseUrl}/Purchase/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Purchase> CreateAsync(Domain.Purchase mPurchase)
        {
            return await _httpClientService.PostAsync<Domain.Purchase>(
                url: $"{_baseUrl}/Purchase",
                obj: mPurchase,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Purchase> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Purchase>(
            url: $"{_baseUrl}/Purchase/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Purchase/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }


    }
}
