using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Infrastructure;
using System.Collections.Generic;
using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using System.Dynamic;
using System.Web.Mvc;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class PurchaseController : Controller
    {
        #region Deleration

        private readonly IPurchaseService _service;
        private readonly IPurchaseItemService _purchaseitemService;
        private readonly IItemService _itemService;
        private readonly IDropdownService _dropdownService;

        public PurchaseController(IPurchaseService service, IPurchaseItemService purchaseitemService, IItemService itemService, IDropdownService dropdownService)
        {
            _service = service;
            _purchaseitemService = purchaseitemService;
            _dropdownService = dropdownService;
            _itemService = itemService;
        }

        #endregion

        #region Purchase

        public ActionResult Index()
        {
            return View(new Domain.PurchaseLister());

        }

        public ActionResult List(Domain.PurchaseLister mLister)
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
            ViewBag.Vendors = _dropdownService.GetVendorsOptionsAsync().GetAwaiter().GetResult();

            var mPurchase = new Domain.Purchase();
            mPurchase.Date = DateTime.Now;

            return PartialView("_Create", mPurchase);
        }

        [HttpPost]
        public ActionResult Create(Domain.Purchase mPurchase)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPurchase = StaticMethods.SetAuditOnUpsert(mPurchase);
                mPurchase = _service.CreateAsync(mPurchase).GetAwaiter().GetResult();

                if (mPurchase != null && mPurchase.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Purchase has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Purchase!" });
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
            var mPurchase = new Domain.Purchase();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPurchase = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mPurchase);
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
                    ViewBag.Vendors = _dropdownService.GetVendorsOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Purchase has been deleted!" };
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
        #endregion

        #region GST

        [HttpGet]
        public JsonResult GetGST(int itemId)
        {
            try
            {
                var item = _itemService.GetAsync(itemId).GetAwaiter().GetResult();
                if (item != null)
                {
                    return Json(new
                    {
                        success = true,
                        CGST = item.CGST,
                        SGST = item.SGST,
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
        #endregion

    }
}