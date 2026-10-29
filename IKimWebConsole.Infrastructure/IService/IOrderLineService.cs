using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IOrderLineService
    {
        Task<Domain.OrderLineLister> GetAllAsync(Domain.OrderLineLister mLister);

        Task<Domain.OrderLine> CreateAsync(Domain.OrderLine mOrderLine);

        Task<List<Domain.OrderLine>> GetAsync(int id);

        Task<Domain.OrderLine> GetByAsync(int id);

        Task DeleteAsync(int id);

        Task<decimal> GetPriceAsync(int orderId, int itemId);

        Task<List<Domain.OrderLine>> GetByOrderId(int orderId);

    }
}
