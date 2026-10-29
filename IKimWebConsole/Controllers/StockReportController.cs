using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using System.Web.Mvc;
using System.Linq;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class StockReportController : Controller
    {
        private readonly IStockReportService _service;
        private readonly IDropdownService _dropdownService;

        public StockReportController(IStockReportService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.StockReport());
        }

        public ActionResult List(Domain.StockReportLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.RedirectUrl = Url.Action("Index", "Authentication");
                return PartialView("_Unauthorized");
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
            }
            return PartialView("_List", mLister);
        }


    }
}