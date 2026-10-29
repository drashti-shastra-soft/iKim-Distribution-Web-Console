using IKimWebConsole.Infrastructure.IService;
using System.Configuration;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class ItemService : IItemService
    {

        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ItemService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.ItemLister> GetAllAsync(Domain.ItemLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.ItemLister>(
                 url: $"{_baseUrl}/Item/GetAll",
                 obj: mLister,
                 token: _token
             ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Item> CreateAsync(Domain.Item mItem)
        {
            return await _httpClientService.PostAsync<Domain.Item>(
               url: $"{_baseUrl}/Item",
               obj: mItem,
               token: _token
           ).ConfigureAwait(false);
        }

        public async Task<Domain.Item> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Item>(
                url: $"{_baseUrl}/Item/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Item/{id}/LoginUser/{_loginUserId}",
                token: _token
            ).ConfigureAwait(false);
        }

        

       
    }
}
