using IKimWebConsole.Infrastructure.IService;
using System.Threading.Tasks;
using System.Configuration;

namespace IKimWebConsole.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public UserService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.UserLister> GetAllAsync(Domain.UserLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.UserLister>(
                url: $"{_baseUrl}/User/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.User> CreateAsync(Domain.User mUser)
        {
            return await _httpClientService.PostAsync<Domain.User, Domain.User>(
                url: $"{_baseUrl}/User",
                obj: mUser,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.User> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.User>(
            url: $"{_baseUrl}/User/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/User/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }

        public async Task<bool> CheckEmailAddressExistAsync(Domain.User mUser)
        {
            var isExist = await _httpClientService.PostAsync<Domain.User, bool>(
                url: $"{_baseUrl}/User/CheckEmailAddressExist",
                obj: mUser,
                token: _token
                ).ConfigureAwait(false);
            return isExist;
        }
    }
}
