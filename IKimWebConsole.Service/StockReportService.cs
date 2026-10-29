using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Service
{
    public class StockReportService : IStockReportService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public StockReportService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastructure.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastructure.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastructure.HttpContextHelper.LoginUser.Id;
            }
        }

        public async Task<Domain.StockReportLister> GetAllAsync(Domain.StockReportLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.StockReportLister>(
                      url: $"{_baseUrl}/StockReport/GetAll",
                      obj: mLister,
                      token: _token
              ).ConfigureAwait(false);

            return result;
        }

    }
}
