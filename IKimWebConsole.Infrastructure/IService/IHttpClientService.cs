using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IHttpClientService
    {
        Task<TR> GetAsync<TR>(string url, string token = null);

        Task<T> PostAsync<T>(string url, T obj, string token = null);

        Task<TR> PostAsync<T, TR>(string url, T obj, string token = null);

        Task PutAsync<T>(string url, T obj, string token = null);

        Task DeleteAsync(string url, string token = null);

        Task PatchAsync(string url, string token = null);
    }
}
