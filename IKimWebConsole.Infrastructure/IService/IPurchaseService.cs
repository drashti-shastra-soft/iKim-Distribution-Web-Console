using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IPurchaseService
    {
        Task<Domain.PurchaseLister> GetAllAsync(Domain.PurchaseLister mLister);

        Task<Domain.Purchase> CreateAsync(Domain.Purchase mPurchase);

        Task<Domain.Purchase> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
