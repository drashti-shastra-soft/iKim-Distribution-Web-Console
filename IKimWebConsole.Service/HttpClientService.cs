using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Infrastructure;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net;
using System;

namespace IKimWebConsole.Service
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService()
        {
            _client = HttpClientProvider.Client;
        }

        public async Task<TR> GetAsync<TR>(string url, string token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (token.IsNotNullOrEmpty())
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.GetAsync(url).ConfigureAwait(false);
            var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TR>(contentString);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Authorization has been denied for this request.");
                case HttpStatusCode.BadRequest:
                    throw new Exception(JsonConvert.DeserializeObject<string>(contentString) ?? "Bad Request!");
                default:
                    throw new Exception("Internal server error!");
            }
        }

        public async Task<TR> PostAsync<T, TR>(string url, T obj, string token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))
            {
                if (token.IsNotNullOrEmpty())
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _client.PostAsync(url, content).ConfigureAwait(false);
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<TR>(contentString);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Authorization has been denied for this request.");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(JsonConvert.DeserializeObject<string>(contentString) ?? "Bad Request!");
                }
                else
                {
                    throw new Exception("Internal server error!");
                }
            }
        }

        public async Task<T> PostAsync<T>(string url, T obj, string token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))
            {
                if (token.IsNotNullOrEmpty())
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _client.PostAsync(url, content).ConfigureAwait(false);
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return JsonConvert.DeserializeObject<T>(contentString);
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException("Authorization has been denied for this request.");
                    case HttpStatusCode.BadRequest:
                        throw new BadRequestException(JsonConvert.DeserializeObject<string>(contentString) ?? "Bad Request!");
                    default:
                        throw new Exception("Internal server error!");
                }
            }
        }

        public async Task PutAsync<T>(string url, T obj, string token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))
            {
                if (token.IsNotNullOrEmpty())
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _client.PutAsync(url, content).ConfigureAwait(false);
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode) return;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException("Authorization has been denied for this request.");
                    case HttpStatusCode.BadRequest:
                        throw new BadRequestException(JsonConvert.DeserializeObject<string>(contentString) ?? "Bad Request!");
                    default:
                        throw new Exception("Internal server error!");
                }
            }
        }

        public async Task DeleteAsync(string url, string token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (token.IsNotNullOrEmpty())
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.DeleteAsync(url).ConfigureAwait(false);
            var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) return;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Authorization has been denied for this request.");
                case HttpStatusCode.BadRequest:
                    throw new Exception(JsonConvert.DeserializeObject<string>(contentString) ?? "Bad Request!");
                default:
                    throw new Exception("Internal server error!");
            }
        }

        public async Task PatchAsync(string url, string token = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (token.IsNotNullOrEmpty())
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = null
            };

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) return;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Authorization has been denied for this request.");
                case HttpStatusCode.BadRequest:
                    throw new Exception(JsonConvert.DeserializeObject<string>(contentString) ?? "Bad Request!");
                default:
                    throw new Exception("Internal server error!");
            }
        }
    }
}
