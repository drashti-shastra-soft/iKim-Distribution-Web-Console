using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class PartyOrderLineService : IPartyOrderLineService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PartyOrderLineService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }


        public async Task<Domain.PartyOrderLineLister> GetAllAsync(Domain.PartyOrderLineLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PartyOrderLineLister>(
                 url: $"{_baseUrl}/PartyOrderLine/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        //public async Task<List<Domain.PartyOrderLine>> GetAsync(int id)
        //{
        //    var result = await _httpClientService.GetAsync<List<Domain.PartyOrderLine>>(
        //       url: $"{_baseUrl}/PartyOrderLine/{id}",
        //       token: _token
        //    ).ConfigureAwait(false);
        //    return result;
        //}

        public async Task<Domain.PartyOrderLine> CreateAsync(Domain.PartyOrderLine mPartyOrderLine)
        {
            return await _httpClientService.PostAsync<Domain.PartyOrderLine>(
               url: $"{_baseUrl}/PartyOrderLine",
               obj: mPartyOrderLine,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.PartyOrderLine> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.PartyOrderLine>(
                        url: $"{_baseUrl}/PartyOrderLine/GetPartyOrderLine/{id}",
                        token: _token
                        ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/PartyOrderLine/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }
    }
}
