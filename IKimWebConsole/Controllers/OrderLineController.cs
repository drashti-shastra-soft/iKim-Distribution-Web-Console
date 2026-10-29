using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class OrderLineController : Controller
    {
        private readonly IOrderLineService _service;
        private readonly IDropdownService _dropdownService;

        public OrderLineController(IOrderLineService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index(int orderId, string store)
        {
            if (orderId <= default(int))
                return RedirectToAction("Index", "Order");

            var mLister = new Domain.OrderLineLister();
            mLister.SearchCriteria.OrderId = orderId;
            mLister.Store = store;
            TempData["OrderId"] = orderId;
            return View(mLister);
        }

        public ActionResult List(Domain.OrderLineLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister.SearchCriteria.OrderId = Convert.ToInt32(TempData.Peek("OrderId"));
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
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
            var mOrderLine = new Domain.OrderLine();
            mOrderLine.OrderId = Convert.ToInt32(TempData.Peek("OrderId"));

            ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();

            return PartialView("_Create", mOrderLine);
        }

        [HttpPost]
        public ActionResult Create(Domain.OrderLine mOrderLine)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);

                var itemCount = mOrderLine.OrderLineItems?.Count ?? 0;

                if (mOrderLine == null)
                {
                    return Json(new { NotificationType = "error", NotificationMsg = "Contract data not found!" }, JsonRequestBehavior.AllowGet);
                }

                mOrderLine.OrderLineItems = mOrderLine.OrderLineItems ?? new List<Domain.OrderLine>();


                mOrderLine = StaticMethods.SetAuditOnUpsert(mOrderLine);
                mOrderLine = _service.CreateAsync(mOrderLine).GetAwaiter().GetResult();

                if (mOrderLine != null && mOrderLine.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Order Line has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Order Line!" });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") });
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "This Order Line is already added to this stock!" });
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mOrderLine = new Domain.OrderLine();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrderLine = _service.GetByAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mOrderLine);
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Order Line has been deleted successfully!" };
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
        public ActionResult GetOrderLineItemRowPartial(int index)
        {
            try
            {
                ViewBag.ItemIndex = index;

                ViewBag.Items = _dropdownService.GetItemOptionsAsync(HttpContextHelper.LoginUser.PricingTierId).GetAwaiter().GetResult();

                return PartialView("_CreateOrderLineItem", new Domain.OrderLine());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}