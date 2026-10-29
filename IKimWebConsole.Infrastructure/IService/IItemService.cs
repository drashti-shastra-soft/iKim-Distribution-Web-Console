using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IItemService
    {
        Task<Domain.ItemLister> GetAllAsync(Domain.ItemLister mLister);

        Task<Domain.Item> CreateAsync(Domain.Item mItem);

        Task<Domain.Item> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
