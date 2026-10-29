using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class VendorService : IVendorService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public VendorService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.VendorLister> GetAllAsync(Domain.VendorLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.VendorLister>(
                 url: $"{_baseUrl}/Vendor/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Vendor> CreateAsync(Domain.Vendor mVendor)
        {
            return await _httpClientService.PostAsync<Domain.Vendor>(
               url: $"{_baseUrl}/Vendor",
               obj: mVendor,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.Vendor> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Vendor>(
                url: $"{_baseUrl}/Vendor/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Vendor/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
