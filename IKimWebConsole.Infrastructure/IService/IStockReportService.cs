using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IStockReportService
    {
        Task<Domain.StockReportLister> GetAllAsync(Domain.StockReportLister mLister);
    }
}
