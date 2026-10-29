using System.Collections.Generic;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IDailyOrderService
    {
        //Task<List<Domain.DailyOrder>> DailyOrdersAsync();


        //Task<List<Domain.Store>> DailyOrdersAsync();

        Task<List<Domain.DailyOrder>> DailyOrdersAsync(Domain.DailyOrder mDailyOrders);

        Task<string> GetPdfAsync(List<Domain.DailyOrder> mDailyOrder);

        Task<List<Domain.DailyOrder>> GetSelectedDailyOrdersAsync(List<int> mDailyOrder);

        //Task<List<Domain.DailyOrder>> DailyOrdersAsync(mDailyOrders);
    }
}
