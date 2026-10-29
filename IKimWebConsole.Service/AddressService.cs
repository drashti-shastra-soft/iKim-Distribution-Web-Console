using IKimWebConsole.Infrastructure.IService;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class AddressService : IAddressService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public AddressService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.Address> CreateAsync(Domain.Address mAddress)
        {
            return await _httpClientService.PostAsync<Domain.Address, Domain.Address>(
                url: $"{_baseUrl}/Address",
                obj: mAddress,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Address> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Address>(
                url: $"{_baseUrl}/Address/Get/Store/Id/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Address> GetVendor(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Address>(
                    url: $"{_baseUrl}/Address/Get/Vendor/Id/{id}",
                    token: _token
            ).ConfigureAwait(false);

            return result;
        }

    }
}
