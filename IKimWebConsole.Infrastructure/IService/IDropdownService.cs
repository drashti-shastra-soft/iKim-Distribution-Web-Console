using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IDropdownService
    {
        dynamic GetStatusOptions();

        dynamic GetPageSizeOptions(int selectedSize = 20);

        Task<dynamic> GetUserTypeOptionsAsync();

        Task<dynamic> GetStoreTypeOptionsAsync();

        Task<dynamic> GetPriceTierOptionsAsync();

        Task<dynamic> GetParentStoresOptionsAsync();

        Task<dynamic> GetStoreOptionsAsync();

        Task<dynamic> GetItemOptionsAsync(int priceTierId);

        Task<dynamic> GetUnitOfMeasureOptionsAsync();

        Task<dynamic> GetCategoryOptionsAsync();

        Task<dynamic> GetStockOptionsAsync();

        Task<dynamic> GetVendorsOptionsAsync();

        dynamic GetWithWaiterOptions();

        Task<dynamic> GetRawMaterialOptionsAsync();
    }
}
