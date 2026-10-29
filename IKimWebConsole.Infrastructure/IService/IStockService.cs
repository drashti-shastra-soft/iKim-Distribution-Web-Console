using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IStockService
    {
        Task<Domain.StockLister> GetAllAsync(Domain.StockLister mLister);

        Task<Domain.Stock> CreateAsync(Domain.Stock mStock);

        Task<Domain.Stock> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
