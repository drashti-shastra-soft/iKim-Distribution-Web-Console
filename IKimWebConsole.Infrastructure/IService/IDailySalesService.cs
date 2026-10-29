using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IDailySalesService
    {
        Task<Domain.DailySalesLister> GetAllAsync(Domain.DailySalesLister mLister);
    }
}
