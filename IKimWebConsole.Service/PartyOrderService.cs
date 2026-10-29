using IKimWebConsole.Domain;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class PartyOrderService : IPartyOrderService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PartyOrderService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.PartyOrderLister> GetAllAsync(Domain.PartyOrderLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PartyOrderLister>(
                url: $"{_baseUrl}/PartyOrder/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.PartyOrder> CreateAsync(Domain.PartyOrder mPartyOrder)
        {
            return await _httpClientService.PostAsync<Domain.PartyOrder, Domain.PartyOrder>(
                url: $"{_baseUrl}/PartyOrder",
                obj: mPartyOrder,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.PartyOrder> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.PartyOrder>(
            url: $"{_baseUrl}/PartyOrder/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/PartyOrder/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }

    }
}
