using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IStockItemService
    {
        Task<Domain.StockItemLister> GetAllAsync(Domain.StockItemLister mLister);
        Task<Domain.StockItem> CreateAsync(Domain.StockItem mStockItem);
        Task<Domain.StockItem> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
