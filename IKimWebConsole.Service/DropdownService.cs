using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Mvc;
using System;

namespace IKimWebConsole.Service
{
    public class DropdownService : IDropdownService
    {
        #region Declaration

        private readonly IHttpClientService _httpClientService;
        private readonly string baseUrl;

        public DropdownService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");
        }

        #endregion

        #region Status
        public dynamic GetStatusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Active", Value = "Active" },
                new SelectListItem { Text = "Disabled", Value = "Disabled" }
            };
        }

        #endregion

        #region Pagenation

        public dynamic GetPageSizeOptions(int selectedSize = 20)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "20", Value = "20" },
                new SelectListItem { Text = "50", Value = "50" },
                new SelectListItem { Text = "100", Value = "100" },
                new SelectListItem { Text = "All", Value = "-1" }
            };

            list.ForEach(item => item.Selected = item.Value == selectedSize.ToString());
            return list;
        }

        #endregion

        #region User Type
        public async Task<dynamic> GetUserTypeOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mUserTypes = await _httpClientService.GetAsync<List<Domain.UserType>>(
                url: $"{baseUrl}/UserType",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mUserType in mUserTypes)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mUserType.Name, Value = Convert.ToString(mUserType.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Store Type
        public async Task<dynamic> GetStoreTypeOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mStoreTypes = await _httpClientService.GetAsync<List<Domain.StoreType>>(
                    url: $"{baseUrl}/StoreType",
                    token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mStoreType in mStoreTypes)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mStoreType.Name, Value = Convert.ToString(mStoreType.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Pricing Tier
        public async Task<dynamic> GetPriceTierOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mPricingTiers = await _httpClientService.GetAsync<List<Domain.PricingTier>>(
                    url: $"{baseUrl}/PricingTier",
                    token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mPricingTier in mPricingTiers)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mPricingTier.Name, Value = Convert.ToString(mPricingTier.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Store Type
        public async Task<dynamic> GetParentStoresOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mParentStores = await _httpClientService.GetAsync<List<Domain.Store>>(
                    url: $"{baseUrl}/Store/GetAllParentStore",
                    token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mMainStore in mParentStores)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mMainStore.Name, Value = Convert.ToString(mMainStore.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Store
        public async Task<dynamic> GetStoreOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mStores = await _httpClientService.GetAsync<List<Domain.Store>>(
                            url: $"{baseUrl}/Store/GetActiveStore",
                            token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mStore in mStores)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mStore.Name, Value = Convert.ToString(mStore.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Item

        public async Task<dynamic> GetItemOptionsAsync(int priceTierId)
        {
            var mSelectListItems = new List<SelectListItem>();

            var mItems = await _httpClientService.GetAsync<List<Domain.Item>>(
                url: $"{baseUrl}/Item/PricingTier/{priceTierId}",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mtem in mItems)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mtem.Name, Value = Convert.ToString(mtem.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Unit Of Measure 
        public async Task<dynamic> GetUnitOfMeasureOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mUnitOfMeasures = await _httpClientService.GetAsync<List<Domain.UnitOfMeasure>>(
                            url: $"{baseUrl}/UnitOfMeasure/GetActiveUnitOfMeasure",
                            token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mUnitOfMeasurement in mUnitOfMeasures)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mUnitOfMeasurement.Name, Value = Convert.ToString(mUnitOfMeasurement.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Category
        public async Task<dynamic> GetCategoryOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mCategorys = await _httpClientService.GetAsync<List<Domain.Category>>(
                            url: $"{baseUrl}/Category",
                            token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mCategory in mCategorys)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mCategory.Name, Value = Convert.ToString(mCategory.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Stock
        public async Task<dynamic> GetStockOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mStocks = await _httpClientService.GetAsync<List<Domain.Stock>>(
                            url: $"{baseUrl}/Stock/GetActiveStock",
                            token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mStock in mStocks)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mStock.Store, Value = Convert.ToString(mStock.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region Vendor
        public async Task<dynamic> GetVendorsOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mVendors = await _httpClientService.GetAsync<List<Domain.Vendor>>(
                            url: $"{baseUrl}/Vendor/GetActiveVendor",
                            token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mtem in mVendors)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mtem.Name, Value = Convert.ToString(mtem.Id) });
            }
            return mSelectListItems;
        }
        #endregion

        #region WithWaiter

        public dynamic GetWithWaiterOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Yes", Value = "Yes" },
                new SelectListItem { Text = "No", Value = "No" }
            };
        }

        #endregion

        #region Raw Material
        public async Task<dynamic> GetRawMaterialOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mItems = await _httpClientService.GetAsync<List<Domain.Item>>(
                                            url: $"{baseUrl}/Item/GetActiveByIsRawMaterial",
                                            token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mItem in mItems)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mItem.Name, Value = Convert.ToString(mItem.Id) });
            }
            return mSelectListItems;
        }
        #endregion

    }
}
