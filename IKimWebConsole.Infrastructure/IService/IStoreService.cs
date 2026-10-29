using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IStoreService
    {
        Task<Domain.StoreLister> GetAllAsync(Domain.StoreLister mLister);

        Task<Domain.Store> CreateAsync(Domain.Store mStore);

        Task<Domain.Store> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
