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
    public class VendorController : Controller
    {
        #region Declaration
        private readonly IVendorService _service;
        private readonly IAddressService _addressService;
        private readonly IDropdownService _dropdownService;

        public VendorController(IVendorService service, IAddressService addressService, IDropdownService dropdownService)
        {
            _service = service;
            _addressService = addressService;
            _dropdownService = dropdownService;
        }

        #endregion

        #region Vendor
        public ActionResult Index()
        {
            return View(new Domain.VendorLister());
        }

        public ActionResult List(Domain.VendorLister mLister)
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
            return PartialView("_Create", new Domain.Vendor());
        }

        [HttpPost]
        public ActionResult Create(Domain.Vendor mVendor)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mVendor = StaticMethods.SetAuditOnUpsert(mVendor);
                mVendor = _service.CreateAsync(mVendor).GetAwaiter().GetResult();

                if (mVendor != null && mVendor.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Vendor has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Vendor!" });
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
            var mVendor = new Domain.Vendor();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mVendor = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mVendor);
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

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Vendor has been deleted successfully!" };
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

        #region Address

        [HttpGet]
        public ActionResult GetAddress(int id)
        {
            dynamic resultData = new ExpandoObject();
            var mAddress = new Domain.Address();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAddress = _addressService.GetVendor(id).GetAwaiter().GetResult();
                mAddress = mAddress ?? new Domain.Address();
                mAddress.VendorId = id;
                return PartialView("_Address", mAddress);
            }
            catch (UnauthorizedAccessException)
            {
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
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Address has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Address!" });
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


        //[HttpGet]
        //public ActionResult GetAddress(int id)
        //{
        //    var mAddress = new Domain.Address();
        //    try
        //    {
        //        mAddress = _addressService.GetVendor(id).GetAwaiter().GetResult();
        //        mAddress = mAddress ?? new Domain.Address();
        //        mAddress.VendorId = id;
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.NotificationType = "error";
        //        ViewBag.NotificationMsg = "Internal server error!";
        //    }
        //    return PartialView("_Address", mAddress);
        //}

        //[HttpPost]
        //public ActionResult SaveAddress(Domain.Address mAddress)
        //{
        //    try
        //    {
        //        System.Threading.Thread.Sleep(1000);
        //        mAddress = StaticMethods.SetAuditOnUpsert(mAddress);
        //        mAddress = _addressService.CreateAsync(mAddress).GetAwaiter().GetResult();

        //        if (mAddress != null && mAddress.Id > 0)
        //        {
        //            ViewBag.NotificationType = "success";
        //            ViewBag.NotificationMsg = "Address has been saved successfully!";
        //        }
        //        else
        //        {
        //            ViewBag.NotificationType = "error";
        //            ViewBag.NotificationMsg = "Error occurred while saving Address!";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.NotificationType = "error";
        //        ViewBag.NotificationMsg = "Internal server error!";
        //    }
        //    return PartialView("_Address", mAddress);
        //}

        #endregion
    }
}