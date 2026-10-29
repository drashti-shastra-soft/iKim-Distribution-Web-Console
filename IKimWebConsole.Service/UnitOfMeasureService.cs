using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public UnitOfMeasureService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.UnitOfMeasureLister> GetAllAsync(Domain.UnitOfMeasureLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.UnitOfMeasureLister>(
                url: $"{_baseUrl}/UnitOfMeasure/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.UnitOfMeasure> CreateAsync(Domain.UnitOfMeasure mUnitOfMeasure)
        {
            return await _httpClientService.PostAsync<Domain.UnitOfMeasure, Domain.UnitOfMeasure>(
                url: $"{_baseUrl}/UnitOfMeasure",
                obj: mUnitOfMeasure,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.UnitOfMeasure> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.UnitOfMeasure>(
            url: $"{_baseUrl}/UnitOfMeasure/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/UnitOfMeasure/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }

    }
}
