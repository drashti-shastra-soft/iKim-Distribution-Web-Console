using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IPartyOrderLineService
    {

        Task<Domain.PartyOrderLineLister> GetAllAsync(Domain.PartyOrderLineLister mLister);

        Task<Domain.PartyOrderLine> CreateAsync(Domain.PartyOrderLine mPartyOrderLine);

        //Task<List<Domain.PartyOrderLine>> GetAsync(int id);

        Task<Domain.PartyOrderLine> GetAsync(int id);

        Task DeleteAsync(int id);
    }
}
