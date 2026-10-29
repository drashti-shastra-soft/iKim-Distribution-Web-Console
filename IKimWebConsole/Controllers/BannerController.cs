using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class BannerController : Controller
    {
        #region Declaration

        private readonly IBannerService _service;
        private readonly IDropdownService _dropdownService;

        public BannerController(IBannerService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        #endregion

        #region Action Methods
        public ActionResult Index()
        {
            return View(new Domain.BannerLister());
        }


        public ActionResult List(Domain.BannerLister mLister)
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
            return PartialView("_Create", new Domain.Banner());
        }

        [HttpPost]
        public ActionResult Create(List<HttpPostedFileBase> file)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                if (file != null && file.Count > 0)
                {
                    bool isSaved = false;
                    var bannerList = _service.GetAllAsync(new Domain.BannerLister()).GetAwaiter().GetResult();
                    int sequence = 1;
                    if (bannerList != null && bannerList.List != null && bannerList.List.Count > 0)
                    {
                        sequence = bannerList.List.Max(x => x.Sequence) + 1;
                    }
                    foreach (var item in file)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                item.InputStream.CopyTo(ms);
                                var fileBytes = ms.ToArray();

                                var mBanner = new Domain.Banner
                                {
                                    FileName = Path.GetFileName(item.FileName),
                                    Base64String = Convert.ToBase64String(fileBytes),
                                    Sequence = sequence,
                                    MimeType = item.ContentType,
                                    IsActive = true
                                };

                                Infrastructure.Helper.AuditHelper.ApplyAuditInfo(mBanner);
                                mBanner = _service.CreateAsync(mBanner).GetAwaiter().GetResult();
                                if (mBanner != null && mBanner.Id > 0)
                                {
                                    isSaved = true;
                                    sequence++;
                                }
                            }
                        }
                    }

                    if (isSaved)
                    {
                        resultData = new { type = "success", message = "Banner uploaded successfully." };
                    }
                    else
                    {
                        resultData = new { type = "error", message = "Banner save failed." };
                    }
                }
                else
                {
                    resultData = new { type = "error", message = "Please select image." };
                }
            }
            catch (BadRequestException ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }
            catch (Exception ex)
            {
                resultData = new { type = "error", message = ex.Message };
            }

            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Banner has been deleted!" };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new { StatusCode = "Unauthorized" };
            }
            catch (BadRequestException ex)
            {
                resultData = new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting banner!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}