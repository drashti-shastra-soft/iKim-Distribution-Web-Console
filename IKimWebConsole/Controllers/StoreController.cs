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
    public class StoreController : Controller
    {
        private readonly IStoreService _service;
        private readonly IDropdownService _dropdownService;
        private readonly IAddressService _addressService;

        public StoreController(IStoreService service, IDropdownService dropdownService, IAddressService addressService)
        {
            _service = service;
            _dropdownService = dropdownService;
            _addressService = addressService;
        }

        public ActionResult Index()
        {
            return View(new Domain.StoreLister());
        }

        public ActionResult List(Domain.StoreLister mLister)
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
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
            ViewBag.StoreTypes = _dropdownService.GetStoreTypeOptionsAsync().GetAwaiter().GetResult();
            ViewBag.ParentStores = _dropdownService.GetParentStoresOptionsAsync().GetAwaiter().GetResult();

            return PartialView("_Create", new Domain.Store());
        }

        [HttpPost]
        public ActionResult Create(Domain.Store mStore)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStore = StaticMethods.SetAuditOnUpsert(mStore);
                mStore = _service.CreateAsync(mStore).GetAwaiter().GetResult();

                if (mStore != null && mStore.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Store has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Store!" });
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
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
                ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                ViewBag.StoreTypes = _dropdownService.GetStoreTypeOptionsAsync().GetAwaiter().GetResult();
                ViewBag.ParentStores = _dropdownService.GetParentStoresOptionsAsync().GetAwaiter().GetResult();
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mStore = new Domain.Store();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mStore = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mStore);
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
                    ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                    ViewBag.StoreTypes = _dropdownService.GetStoreTypeOptionsAsync().GetAwaiter().GetResult();
                    ViewBag.ParentStores = _dropdownService.GetParentStoresOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Store has been deleted successfully!" };
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

        #region Address

        [HttpGet]
        public ActionResult GetAddress(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                mAddress = _addressService.GetAsync(id).GetAwaiter().GetResult();
                mAddress = mAddress ?? new Domain.Address();
                mAddress.StoreId = id;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }


        [HttpPost]
        public ActionResult SaveAddress(Domain.Address mAddress)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAddress = StaticMethods.SetAuditOnUpsert(mAddress);
                mAddress = _addressService.CreateAsync(mAddress).GetAwaiter().GetResult();

                if (mAddress != null && mAddress.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Address has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Address!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }

        #endregion
    }
}