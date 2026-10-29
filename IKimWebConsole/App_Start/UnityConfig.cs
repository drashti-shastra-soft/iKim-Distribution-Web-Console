using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Service;
using System.Web.Mvc;
using Unity.Mvc5;
using Unity;

namespace IKimWebConsole
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IBannerService, BannerService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IDailyOrderService, DailyOrderService>();
            container.RegisterType<IDailySalesService, DailySalesService>();
            container.RegisterType<IDashboardService, DashboardService>();
            container.RegisterType<IDropdownService, DropdownService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IHttpClientService, HttpClientService>();
            container.RegisterType<IItemFulfillmentService, ItemFulfillmentService>();
            container.RegisterType<IItemPriceService, ItemPriceService>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<INotificationService, NotificationService>();
            container.RegisterType<IOrderLineService, OrderLineService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IPartyOrderLineService, PartyOrderLineService>();
            container.RegisterType<IPartyOrderService, PartyOrderService>();
            container.RegisterType<IPurchaseItemService, PurchaseItemService>();
            container.RegisterType<IPurchaseService, PurchaseService>();
            container.RegisterType<IStockItemService, StockItemService>();
            container.RegisterType<IStockReportService, StockReportService>();
            container.RegisterType<IStockService, StockService>();
            container.RegisterType<IStockTransferService, StockTransferService>();
            container.RegisterType<IStockUpdateService, StockUpdateService>();
            container.RegisterType<IStoreService, StoreService>();
            container.RegisterType<IUnitOfMeasureService, UnitOfMeasureService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IVendorService, VendorService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}