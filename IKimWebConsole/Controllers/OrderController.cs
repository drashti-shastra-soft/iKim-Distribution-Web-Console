using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class OrderController : Controller
    {
        #region Declaration

        private readonly IOrderService _service;
        private readonly IOrderLineService _orderlineService;
        private readonly IItemFulfillmentService _itemfulfillmentService;
        private readonly IItemService _itemService;
        private readonly IDailyOrderService _dailyOrderService;
        private readonly IDropdownService _dropdownService;

        public OrderController(IOrderService service, IOrderLineService orderlineService, IItemFulfillmentService itemfulfillmentService, IDropdownService dropdownService, IItemService itemService, IDailyOrderService dailyOrderService)
        {
            _service = service;
            _orderlineService = orderlineService;
            _itemfulfillmentService = itemfulfillmentService;
            _dropdownService = dropdownService;
            _itemService = itemService;
            _dailyOrderService = dailyOrderService;
        }

        #endregion

        #region Order
        public ActionResult Index()
        {
            return View(new Domain.OrderLister());
        }

        public ActionResult List(Domain.OrderLister mLister)
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
            ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();

            var mOrder = new Domain.Order();
            mOrder.Date = DateTime.Now;

            return PartialView("_Create", mOrder);
        }

        [HttpPost]
        public ActionResult Create(Domain.Order mOrder)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrder = StaticMethods.SetAuditOnUpsert(mOrder);
                mOrder = _service.CreateAsync(mOrder).GetAwaiter().GetResult();

                if (mOrder != null && mOrder.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Order has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Order!" });
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
            var mOrder = new Domain.Order();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrder = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mOrder);
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
                    ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Order has been deleted successfully!" };
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

        #region PDF

        [HttpPost]
        public ActionResult DownloadMergedPdf(List<Domain.Order> orders)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                var base64String = _service.GetPdfAsync(orders).GetAwaiter().GetResult();
                resultData = new { StatusCode = "Ok", Base64String = base64String };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new { StatusCode = "Unauthorized" };
            }
            catch (Exception ex)
            {
                resultData = new { StatusCode = "InternalServerError", Message = ex.Message };
            }

            return Json(resultData);
        }

        //[HttpPost]
        //public async Task<JsonResult> DownloadSelectedDailyOrders(List<Domain.DailyOrder> mDailyOrder)
        //{
        //    try
        //    {
        //        if (mDailyOrder == null || mDailyOrder.Count == 0)
        //        {
        //            return Json(new { StatusCode = "BadRequest",Message = "Please select at least one order."});
        //        }
        //        var result = await _dailyOrderService.GetPdfAsync(mDailyOrder);
        //        if (string.IsNullOrEmpty(result))
        //        {
        //            return Json(new { StatusCode = "BadRequest",Message = "Daily Order PDF could not be generated." });
        //        }

        //        return Json(new { StatusCode = "Ok", Base64String = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { StatusCode = "InternalServerError", Message = ex.Message }); 
        //    }
        //}


        [HttpPost]
        public async Task<JsonResult> DownloadSelectedDailyOrders(List<Domain.Order> selectedItems)
        {
            try
            {
                if (selectedItems == null || !selectedItems.Any())
                {
                    return Json(new{ StatusCode = "BadRequest", Message = "Please select at least one order."});
                }

                var orderIds = selectedItems.Select(x => x.Id).Distinct().ToList();

                var dailyOrders = await _dailyOrderService.GetSelectedDailyOrdersAsync(orderIds);

                if (dailyOrders == null || !dailyOrders.Any())
                {
                    return Json(new { StatusCode = "BadRequest", Message = "No Order Line data found for selected orders." });
                }

                var base64String = await _dailyOrderService.GetPdfAsync(dailyOrders);

                if (string.IsNullOrEmpty(base64String))
                {
                    return Json(new{StatusCode = "BadRequest", Message = "Failed to generate Daily Order PDF." });
                }
                return Json(new { StatusCode = "Ok", Base64String = base64String });
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", Message = "Failed to download Daily Order." });
            }
        }

        #endregion

        #region Fulfillment

        [HttpGet]
        public ActionResult CreateFulfillment(int orderId)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mOrderLines = new List<Domain.OrderLine>();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mOrderLines = _orderlineService.GetByOrderId(orderId).GetAwaiter().GetResult();
                ViewBag.OrderId = orderId;
                return PartialView("_CreateFulfillment", mOrderLines);
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
                }
            }
        }

        [HttpPost]
        public ActionResult SaveFulfillment(Domain.ItemFulfillment mItemFulfillments)
        {
            dynamic resultData = new System.Dynamic.ExpandoObject();
            try
            {
                mItemFulfillments = StaticMethods.SetAuditOnUpsert(mItemFulfillments);
                mItemFulfillments = _itemfulfillmentService.CreateAsync(mItemFulfillments).GetAwaiter().GetResult();

                if (mItemFulfillments != null && mItemFulfillments.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Item Fulfillment has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving item fulfillment!" });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") });
            }
            catch (BadRequestException ex)
            {
                return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" });
            }
        }

        #endregion

        #region Price

        [HttpGet]
        public JsonResult GetPrice(int orderId, int itemId)
        {
            try
            {
                var price = _orderlineService.GetPriceAsync(orderId, itemId).GetAwaiter().GetResult();
                return Json(price, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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

        [HttpGet]
        public JsonResult GetFactoryStock(int itemId)
        {
            var item = _itemService.GetAsync(itemId).GetAwaiter().GetResult();

            return Json(new { Stock = item.FactoryStock }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }   
}