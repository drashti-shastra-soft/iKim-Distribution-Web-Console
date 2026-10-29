using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public NotificationService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.NotificationLister> GetAllAsync(Domain.NotificationLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.NotificationLister>(
                url: $"{_baseUrl}/Notification/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Notification> CreateAsync(Domain.Notification mNotification)
        {
            return await _httpClientService.PostAsync<Domain.Notification, Domain.Notification>(
                url: $"{_baseUrl}/Notification",
                obj: mNotification,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Notification> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Notification>(
            url: $"{_baseUrl}/Notification/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Notification/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
