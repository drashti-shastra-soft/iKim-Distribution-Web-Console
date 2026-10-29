using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IUnitOfMeasureService
    {
        Task<Domain.UnitOfMeasureLister> GetAllAsync(Domain.UnitOfMeasureLister mLister);
        Task<Domain.UnitOfMeasure> CreateAsync(Domain.UnitOfMeasure mUnitOfMeasure);
        Task<Domain.UnitOfMeasure> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
