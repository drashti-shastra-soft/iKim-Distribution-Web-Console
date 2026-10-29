using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IBannerService
    {
        Task<Domain.BannerLister> GetAllAsync(Domain.BannerLister mLister);

        Task<Domain.Banner>CreateAsync(Domain.Banner mBanner);

        Task DeleteAsync(int id);
    }
}
