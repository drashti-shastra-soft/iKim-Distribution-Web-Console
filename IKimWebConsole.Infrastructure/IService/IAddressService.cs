using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IAddressService
    {
        Task<Domain.Address> CreateAsync(Domain.Address mAddress);
        Task<Domain.Address> GetAsync(int id);
        Task<Domain.Address> GetVendor(int id);
    }
}
