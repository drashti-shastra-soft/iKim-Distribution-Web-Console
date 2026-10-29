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
    public class PurchaseItemController : Controller
    {
        private readonly IPurchaseItemService _service;
        private readonly IDropdownService _dropdownService;

        public PurchaseItemController(IPurchaseItemService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index(int purchaseId, string vendor)
        {
            if (purchaseId <= default(int))
                return RedirectToAction("Index", "Purchase");

            var mLister = new Domain.PurchaseItemLister();
            mLister.SearchCriteria.PurchaseId = purchaseId;
            mLister.Vendor = vendor;
            TempData["PurchaseId"] = purchaseId;
            return View(mLister);
        }

        public ActionResult List(Domain.PurchaseItemLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.PurchaseId = Convert.ToInt32(TempData.Peek("PurchaseId"));
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
            var mPurchaseItem = new Domain.PurchaseItem();
            mPurchaseItem.PurchaseId = Convert.ToInt32(TempData.Peek("PurchaseId"));

            ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();
            ViewBag.RawMaterials = _dropdownService.GetRawMaterialOptionsAsync().GetAwaiter().GetResult();
            ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();

            return PartialView("_Create", mPurchaseItem);
        }

        [HttpPost]
        public ActionResult Create(Domain.PurchaseItem mPurchaseItem)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPurchaseItem = StaticMethods.SetAuditOnUpsert(mPurchaseItem);
                mPurchaseItem = _service.CreateAsync(mPurchaseItem).GetAwaiter().GetResult();

                if (mPurchaseItem != null && mPurchaseItem.Id > 0)
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
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal Server Error!" });
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mPurchaseItem = new Domain.PurchaseItem();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPurchaseItem = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mPurchaseItem);
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
                    ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();
                    ViewBag.RawMaterials = _dropdownService.GetRawMaterialOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Purchase Item has been deleted successfully!" };
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