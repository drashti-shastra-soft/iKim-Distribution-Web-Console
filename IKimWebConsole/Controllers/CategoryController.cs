using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        private readonly IDropdownService _dropdownService;

        public CategoryController(ICategoryService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.CategoryLister());
        }

        public ActionResult List(Domain.CategoryLister mLister)
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
            return PartialView("_Create", new Domain.Category());
        }

        [HttpPost]
        public ActionResult Create(Domain.Category mCategory, HttpPostedFileBase file)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                if (file != null && file.ContentLength > 0)
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        mCategory.FileName = System.IO.Path.GetFileName(file.FileName);
                        mCategory.Base64String = Convert.ToBase64String(fileBytes);
                        mCategory.MimeType = file.ContentType;
                    }
                }
                mCategory = StaticMethods.SetAuditOnUpsert(mCategory);
                mCategory = _service.CreateAsync(mCategory).GetAwaiter().GetResult();

                if (mCategory != null && mCategory.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Category has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Category!" });
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
            var mCategory = new Domain.Category();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mCategory = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mCategory);
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Category has been deleted successfully!" };
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