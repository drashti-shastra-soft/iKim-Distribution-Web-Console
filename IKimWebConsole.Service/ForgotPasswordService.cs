using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;

        public ForgotPasswordService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");
        }

        public async Task Initiate(Domain.ForgotPassword mForgotPassword)
        {
            await _httpClientService.PostAsync<Domain.ForgotPassword>(url: $"{_baseUrl}/ForgotPassword/", mForgotPassword).ConfigureAwait(false);
        }
    }
}
