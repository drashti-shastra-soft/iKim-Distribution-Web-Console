using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IPurchaseItemService
    {
        Task<Domain.PurchaseItemLister> GetAllAsync(Domain.PurchaseItemLister mLister);

        Task<List<Domain.PurchaseItem>> GetByAsync(int id);

        Task<Domain.PurchaseItem> CreateAsync(Domain.PurchaseItem mPurchaseItem);

        Task<Domain.PurchaseItem> GetAsync(int id);

        Task DeleteAsync(int id);

        Task<decimal> GetPriceAsync(int purchaseId, int itemId);

    }
}
