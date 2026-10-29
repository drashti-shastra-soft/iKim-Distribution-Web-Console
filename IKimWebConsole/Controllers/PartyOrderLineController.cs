using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class PartyOrderLineController : Controller
    {
        private readonly IPartyOrderLineService _service;
        private readonly IPartyOrderService _partyOrderService;
        private readonly IItemService _itemService;
        private readonly IDropdownService _dropdownService;

        public PartyOrderLineController(IPartyOrderLineService service, IDropdownService dropdownService, IPartyOrderService partyOrderService, IItemService itemService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _partyOrderService = partyOrderService;
            _itemService = itemService;
        }

        public ActionResult Index(int id)
        {
            if (id <= default(int))
                return RedirectToAction("Index", "PartyOrder");

            var mLister = new Domain.PartyOrderLineLister();
            mLister.SearchCriteria.PartyOrderId = id;
            TempData["PartyOrderId"] = id;
            return View(mLister);
        }

        public ActionResult List(Domain.PartyOrderLineLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.PartyOrderId = Convert.ToInt32(TempData.Peek("PartyOrderId"));
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();

                var store = _partyOrderService.GetAsync(mLister.SearchCriteria.PartyOrderId).GetAwaiter().GetResult();
                ViewBag.Name = store.Name;
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
            }

            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var mPartyOrderLine = new Domain.PartyOrderLine();
            mPartyOrderLine.PartyOrderId = Convert.ToInt32(TempData.Peek("PartyOrderId"));


            ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();
            ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();

            return PartialView("_Create", mPartyOrderLine);
        }

        [HttpPost]
        public ActionResult Create(Domain.PartyOrderLine mPartyOrderLine)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPartyOrderLine = StaticMethods.SetAuditOnUpsert(mPartyOrderLine);
                mPartyOrderLine = _service.CreateAsync(mPartyOrderLine).GetAwaiter().GetResult();

                if (mPartyOrderLine != null && mPartyOrderLine.Id > 0)
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
            var mPartyOrderLine = new Domain.PartyOrderLine();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPartyOrderLine = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mPartyOrderLine);
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