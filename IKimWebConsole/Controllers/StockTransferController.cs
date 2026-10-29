using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Service;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class StockTransferController : Controller
    {
        private readonly IStockTransferService _service;
        private readonly IDropdownService _dropdownService;
        private readonly IItemService _itemService;

        public StockTransferController(IStockTransferService service, IDropdownService dropdownService, IItemService itemService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _itemService = itemService;
        }

        public ActionResult Index()
        {
            return View(new Domain.StockTransferLister());
        }

        public ActionResult List(Domain.StockTransferLister mLister)
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

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();
            ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();

            var mStockTransfer = new Domain.StockTransfer();
            mStockTransfer.Date = DateTime.Now;

            return PartialView("_Create", mStockTransfer);
        }

        [HttpPost]
        public ActionResult Create(Domain.StockTransfer mStockTransfer)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);

                mStockTransfer = _service.CreateAsync(mStockTransfer).GetAwaiter().GetResult();

                if (mStockTransfer != null && mStockTransfer.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Stock Transfer has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Stock Transfer!" });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") });
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" });
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mStockTransfer = new Domain.StockTransfer();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStockTransfer = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mStockTransfer);
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
                    ViewBag.Status = _dropdownService.GetStatusOptions();
                    ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();
                    ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Stock Transfer has been deleted successfully!" };
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

        [HttpGet]
        public JsonResult GetUnitByItem(int itemId)
        {
            try
            {
                var item = _itemService.GetAsync(itemId).GetAwaiter().GetResult();
                if (item != null)
                {
                    return Json(new
                    {
                        success = true,
                        UnitOfMeasureId = item.UnitOfMeasureId,
                        UnitOfMeasure = item.UnitOfMeasure,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { success = false, message = "Unable to fetch GST." }, JsonRequestBehavior.AllowGet);
        }

    }
}