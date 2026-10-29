using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class StockItemService : IStockItemService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public StockItemService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.StockItemLister> GetAllAsync(Domain.StockItemLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.StockItemLister>(
                 url: $"{_baseUrl}/StockItem/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.StockItem> CreateAsync(Domain.StockItem mStockItem)
        {
            return await _httpClientService.PostAsync<Domain.StockItem>(
               url: $"{_baseUrl}/StockItem",
               obj: mStockItem,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.StockItem> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.StockItem>(
                url: $"{_baseUrl}/StockItem/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/StockItem/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
