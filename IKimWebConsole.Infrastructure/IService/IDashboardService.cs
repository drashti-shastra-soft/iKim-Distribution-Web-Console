using System.Collections.Generic;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IDashboardService
    {

        Task<List<Domain.Data>> SalesChartAsync(int storeId);

        Task<List<Domain.Data>> PurchaseChartAsync(int vendorId);

        Task<List<Domain.Data>> IngredientConsumptionReportAsync(int itemId);
    }
}
