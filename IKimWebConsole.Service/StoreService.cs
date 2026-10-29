using IKimWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKimWebConsole.Service
{
    public class StoreService : IStoreService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public StoreService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.StoreLister> GetAllAsync(Domain.StoreLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.StoreLister>(
                url: $"{_baseUrl}/Store/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Store> CreateAsync(Domain.Store mStore)
        {
            return await _httpClientService.PostAsync<Domain.Store, Domain.Store>(
                url: $"{_baseUrl}/Store",
                obj: mStore,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Store> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Store>(
            url: $"{_baseUrl}/store/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Store/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
