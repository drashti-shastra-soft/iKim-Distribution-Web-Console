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
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IDropdownService _dropdownService;

        public UserController(IUserService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.UserLister());
        }

        public ActionResult List(Domain.UserLister mLister)
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
                ViewBag.UserType = _dropdownService.GetUserTypeOptionsAsync().GetAwaiter().GetResult();
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UserType = _dropdownService.GetUserTypeOptionsAsync().GetAwaiter().GetResult();
            ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();

            return PartialView("_Create", new Domain.User());
        }

        [HttpPost]
        public ActionResult Create(Domain.User mUser)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mUser = StaticMethods.SetAuditOnUpsert(mUser);
                mUser = _service.CreateAsync(mUser).GetAwaiter().GetResult();

                if (mUser != null && mUser.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "User has been saved successfully!" }); 
                }
                else 
                { 
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving user!" }); 
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
            var mUser = new Domain.User();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mUser = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mUser);
            }
            catch (UnauthorizedAccessException)
            {
                sessionExpired = true;
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication")  }, JsonRequestBehavior.AllowGet);
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
                    ViewBag.UserType = _dropdownService.GetUserTypeOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "User has been deleted successfully!" };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication")}; 
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

        [HttpPost]
        public ActionResult CheckEmailAddressExist(Domain.User mUser)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                var isExist = _service.CheckEmailAddressExistAsync(mUser).GetAwaiter().GetResult();
                resultData = new { StatusCode = "Ok", IsExist = isExist };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new
                {
                    StatusCode = "Unauthorized",
                    RedirectUrl = Url.Action("Index", "Authentication")
                };
            }
            catch (BadRequestException ex)
            {
                resultData = new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
    }
}