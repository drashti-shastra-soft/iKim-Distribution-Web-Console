using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IStockTransferService
    {
        Task<Domain.StockTransferLister> GetAllAsync(Domain.StockTransferLister mLister);
        Task<Domain.StockTransfer> CreateAsync(Domain.StockTransfer mStockTransfer);
        Task<Domain.StockTransfer> GetAsync(int id);
        Task DeleteAsync(int id);
    }

}
