using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using System.Dynamic;
using System.Web.Mvc;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class StockItemController : Controller
    {
        private readonly IStockItemService _service;
        private readonly IStockService _stockService;
        private readonly IDropdownService _dropdownService;

        public StockItemController(IStockItemService service, IDropdownService dropdownService, IStockService stockService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _stockService = stockService;
        }

        public ActionResult Index(int id)
        {
            if (id <= default(int))
                return RedirectToAction("Index", "Stock");

            var mLister = new Domain.StockItemLister();
            mLister.SearchCriteria.StockId = id;
            TempData["StockId"] = id;
            return View(mLister);
        }

        public ActionResult List(Domain.StockItemLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.StockId = Convert.ToInt32(TempData.Peek("StockId"));
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();

                var store = _stockService.GetAsync(mLister.SearchCriteria.StockId).GetAwaiter().GetResult();
                ViewBag.StoreName = store.Store;
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.RedirectUrl = Url.Action("Index", "Authentication");
                return PartialView("_Unauthorized");
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var mStockItem = new Domain.StockItem();
            mStockItem.StockId = Convert.ToInt32(TempData.Peek("StockId"));

            ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();

            return PartialView("_Create", mStockItem);
        }

        [HttpPost]
        public ActionResult Create(Domain.StockItem mStockItem)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStockItem = StaticMethods.SetAuditOnUpsert(mStockItem);
                mStockItem = _service.CreateAsync(mStockItem).GetAwaiter().GetResult();

                if (mStockItem != null && mStockItem.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Item has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving item!" });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") });
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "This item is already added to this stock!" });
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mStockItem = new Domain.StockItem();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStockItem = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mStockItem);
            }
            catch (UnauthorizedAccessException)
            {
                sessionExpired = true;
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") }, JsonRequestBehavior.AllowGet);
            }
            catch (BadRequestException ex)
            {
                return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (!sessionExpired)
                {
                    ViewBag.Stocks = _dropdownService.GetStockOptionsAsync().GetAwaiter().GetResult();
                    ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Item has been deleted successfully!" };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") };
            }
            catch (BadRequestException ex)
            {
                resultData = new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message };
            }
            catch (Exception)
            {
                resultData = new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
    }
}