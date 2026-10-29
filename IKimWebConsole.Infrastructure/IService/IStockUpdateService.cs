using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IStockUpdateService
    {
        Task<Domain.StockUpdate> CreateAsync(Domain.StockUpdate mStockUpdate);

        Task<List<Domain.StockUpdate>> GetByItemId(int itemId);
    }
}
