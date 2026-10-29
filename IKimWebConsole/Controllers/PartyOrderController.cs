using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class PartyOrderController : Controller
    {
        #region Declaration
        private readonly IPartyOrderService _service;
        private readonly IPartyOrderLineService _partyOrderlineService;
        private readonly IItemService _itemService;
        private readonly IDropdownService _dropdownService;

        public PartyOrderController(IPartyOrderService service, IPartyOrderLineService partyOrderlineService, IDropdownService dropdownService, IItemService itemService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _partyOrderlineService = partyOrderlineService;
            _itemService = itemService;
        }
        #endregion

        #region Party Order 
        public ActionResult Index()
        {
            return View(new Domain.PartyOrderLister());
        }

        public ActionResult List(Domain.PartyOrderLister mLister)
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
            ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();

            return PartialView("_Create", new Domain.PartyOrder());
        }

        [HttpPost]
        public ActionResult Create(Domain.PartyOrder mPartyOrder)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPartyOrder = StaticMethods.SetAuditOnUpsert(mPartyOrder);
                mPartyOrder = _service.CreateAsync(mPartyOrder).GetAwaiter().GetResult();

                if (mPartyOrder != null && mPartyOrder.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Party Order has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Party Order!" });
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
            var mPartyOrder = new Domain.PartyOrder();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mPartyOrder = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mPartyOrder);
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Party Order has been deleted successfully!" };
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
    }
}