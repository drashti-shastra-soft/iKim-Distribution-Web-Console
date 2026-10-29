using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IVendorService
    {
        Task<Domain.VendorLister> GetAllAsync(Domain.VendorLister mLister);
        Task<Domain.Vendor> CreateAsync(Domain.Vendor mVendor);
        Task<Domain.Vendor> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
