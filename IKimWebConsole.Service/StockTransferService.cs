using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class StockTransferService : IStockTransferService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public StockTransferService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.StockTransferLister> GetAllAsync(Domain.StockTransferLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.StockTransferLister>(
                    url: $"{_baseUrl}/StockTransfer/GetAll",
                    obj: mLister,
                    token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.StockTransfer> CreateAsync(Domain.StockTransfer mStockTransfer)
        {
            return await _httpClientService.PostAsync<Domain.StockTransfer, Domain.StockTransfer>(
                    url: $"{_baseUrl}/StockTransfer",
                    obj: mStockTransfer,
                    token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.StockTransfer> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.StockTransfer>(
            url: $"{_baseUrl}/StockTransfer/{id}",
                    token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                    url: $"{_baseUrl}/StockTransfer/{id}/LoginUser/{_loginUserId}",
                    token: _token
              ).ConfigureAwait(false);
        }
    }

}
