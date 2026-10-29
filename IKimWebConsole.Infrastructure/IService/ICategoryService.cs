using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface ICategoryService
    {
        Task<Domain.CategoryLister> GetAllAsync(Domain.CategoryLister mLister);
        Task<Domain.Category> CreateAsync(Domain.Category mCategory);
        Task<Domain.Category> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
