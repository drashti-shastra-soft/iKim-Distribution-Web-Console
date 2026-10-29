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
    public class NotificationController : Controller
    {
        private readonly INotificationService _service;
        private readonly IDropdownService _dropdownService;

        public NotificationController(INotificationService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.NotificationLister());
        }

        public ActionResult List(Domain.NotificationLister mLister)
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
            return PartialView("_Create", new Domain.Notification());
        }

        [HttpPost]
        public ActionResult Create(Domain.Notification mNotification)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mNotification = StaticMethods.SetAuditOnUpsert(mNotification);
                mNotification = _service.CreateAsync(mNotification).GetAwaiter().GetResult();

                if (mNotification != null && mNotification.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Notification has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Notification!" });
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
            var mNotification = new Domain.Notification();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mNotification = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mNotification);
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Notification has been deleted successfully!" };
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