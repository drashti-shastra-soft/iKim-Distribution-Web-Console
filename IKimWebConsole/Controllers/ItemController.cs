using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using IKimWebConsole.Infrastructure;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class ItemController : Controller
    {
        private readonly IItemService _service;
        private readonly IItemPriceService _itemPriceService;
        private readonly IStockUpdateService _stockUpdateService;
        private readonly IDropdownService _dropdownService;

        public ItemController(IItemService service, IItemPriceService itemPriceService, IStockUpdateService stockUpdateService, IDropdownService dropdownService )
        {
            _service = service;
            _dropdownService = dropdownService;
            _itemPriceService = itemPriceService;
            _stockUpdateService = stockUpdateService;
        }

        public ActionResult Index()
        {
            return View(new Domain.ItemLister());
        }


        public ActionResult List(Domain.ItemLister mLister)
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
            ViewBag.UnitOfMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            ViewBag.Categories = _dropdownService.GetCategoryOptionsAsync().GetAwaiter().GetResult();
            var mItem = new Domain.Item();
            var mPricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
            if (mPricingTiers != null)
            {
                foreach (var item in mPricingTiers)
                {
                    mItem.Prices.Add(new Domain.ItemPrice() { PricingTierId = Convert.ToInt32(item.Value), PricingTierName = item.Text });
                }
            }
            return PartialView("_Create", mItem);
        }

        [HttpPost]
        public ActionResult Create(Domain.Item mItem, HttpPostedFileBase file)
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

                        mItem.FileName = System.IO.Path.GetFileName(file.FileName);
                        mItem.Base64String = Convert.ToBase64String(fileBytes);
                        mItem.MimeType = file.ContentType;
                    }
                }
                mItem = StaticMethods.SetAuditOnUpsert(mItem);
                mItem = _service.CreateAsync(mItem).GetAwaiter().GetResult();

                if (mItem != null && mItem.Id > 0)
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
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" });
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mItem = new Domain.Item();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mItem = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mItem);
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
                    ViewBag.PricingTiers = _dropdownService.GetPriceTierOptionsAsync().GetAwaiter().GetResult();
                    ViewBag.Categories = _dropdownService.GetCategoryOptionsAsync().GetAwaiter().GetResult();
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
        public ActionResult GetItemPrice(int id)
        {
            var mItemPrice = new List<Domain.ItemPrice>();

            try
            {
                System.Threading.Thread.Sleep(1000);
                mItemPrice = _itemPriceService.GetByAsync(id).GetAwaiter().GetResult();
                return PartialView("_ItemPrice", mItemPrice);
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

        [HttpGet]
        public ActionResult GetStock(int itemId)
        {
            var mStockUpdates = new List<Domain.StockUpdate>();

            try
            {
                System.Threading.Thread.Sleep(1000);

                mStockUpdates = _stockUpdateService.GetByItemId(itemId).GetAwaiter().GetResult();
                ViewBag.Item = mStockUpdates.FirstOrDefault()?.Item;
                return PartialView("_GetStock", mStockUpdates);
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
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Something went wrong!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}