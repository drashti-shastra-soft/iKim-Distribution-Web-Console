using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public CategoryService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.CategoryLister> GetAllAsync(Domain.CategoryLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.CategoryLister>(
                url: $"{_baseUrl}/Category/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Category> CreateAsync(Domain.Category mCategory)
        {
            return await _httpClientService.PostAsync<Domain.Category, Domain.Category>(
                url: $"{_baseUrl}/Category",
                obj: mCategory,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Category> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Category>(
            url: $"{_baseUrl}/Category/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Category/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
