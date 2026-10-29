using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IPartyOrderService
    {
        Task<Domain.PartyOrderLister> GetAllAsync(Domain.PartyOrderLister mLister);
        Task<Domain.PartyOrder> CreateAsync(Domain.PartyOrder mPartyOrder);
        Task<Domain.PartyOrder> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
