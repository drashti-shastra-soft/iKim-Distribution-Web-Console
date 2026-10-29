using IKimWebConsole.Infrastructure.IService;
using System.Collections.Generic;
using IKimWebConsole.Filter;
using IKimWebConsole.Helper;
using System.Web.Mvc;
using System.Linq;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class StockUpdateController : Controller
    {
        private readonly IItemService _service;
        private readonly IStockUpdateService _stockUpdateService;
        private readonly IDropdownService _dropdownService;
        private readonly ICategoryService _categoryService;

        public StockUpdateController(IItemService service, IStockUpdateService stockUpdateService, IDropdownService dropdownService, ICategoryService categoryService)
        {
            _service = service;
            _stockUpdateService = stockUpdateService;
            _dropdownService = dropdownService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View(new List<Domain.Category>());
        }

        public ActionResult List()
        {
            var mLister = new Domain.CategoryLister();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _categoryService.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.RedirectUrl = Url.Action("Index", "Authentication");
                return PartialView("_Unauthorized");
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }

            return PartialView("_List", mLister.List);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create", new Domain.StockUpdate());
        }

        [HttpPost]
        public ActionResult UpdateStock(Domain.Item mItem, string Reason)
        {
            dynamic resultData = new System.Dynamic.ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                var item = _service.GetAsync(mItem.Id).GetAwaiter().GetResult();
                if (item != null)
                {
                    decimal oldStock = item.FactoryStock ?? 0;
                    decimal newStock = mItem.FactoryStock ?? 0;
                    string operation = "";
                    if (newStock > oldStock)
                    {
                        operation = "Plus";
                    }
                    else if (newStock < oldStock)
                    {
                        operation = "Minus";
                    }
                    else
                    {
                        resultData = new { IsSuccess = false, Message = "No stock change found!" };
                        return Json(resultData, JsonRequestBehavior.AllowGet);
                    }
                    item.FactoryStock = newStock;
                    item = _service.CreateAsync(item).GetAwaiter().GetResult();

                    if (item != null && item.Id > 0)
                    {
                        Domain.StockUpdate stockUpdate = new Domain.StockUpdate()
                        {
                            ItemId = item.Id,
                            Quantity = Math.Abs(newStock - oldStock),
                            Operation = operation,
                            Reason = operation == "Minus" ? Reason : null,
                        };
                        var stockResult = _stockUpdateService.CreateAsync(stockUpdate).GetAwaiter().GetResult();
                        if (stockResult != null && stockResult.Id > 0)
                        {
                            resultData = new { IsSuccess = true, Message = "Stock updated successfully!" };
                        }
                        else
                        {
                            resultData = new { IsSuccess = false, Message = "Stock history save failed!" };
                        }
                    }
                    else
                    {
                        resultData = new { IsSuccess = false, Message = "Item update failed!" };
                    }
                }
                else
                {
                    resultData = new { IsSuccess = false, Message = "Item not found!" };
                }
            }
            catch (Exception ex)
            {
                resultData = new { IsSuccess = false, Message = ex.Message };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Items(int categoryId)
        //{
        //    var mLister = new Domain.ItemLister();
        //    try
        //    {
        //        mLister.SearchCriteria.CategoryId = categoryId;
        //        mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
        //        ViewBag.CategoryId = categoryId;
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        ViewBag.RedirectUrl = Url.Action("Index", "Authentication");
        //        return PartialView("_Unauthorized");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.NotificationType = "error";
        //        ViewBag.NotificationMsg = ex.Message;
        //    }
        //    return View("_Items", mLister.List);
        //}

        public ActionResult Items(int categoryId, int? pageSize, int? skip)
        {
            var mLister = new Domain.ItemLister();

            try
            {
                // Set category filter
                mLister.SearchCriteria.CategoryId = categoryId;
                if (pageSize.HasValue && pageSize.Value > 0) { mLister.Pagination.PageSize = pageSize.Value; }
                if (skip.HasValue && skip.Value >= 0) { mLister.Pagination.Skip = skip.Value; }
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
                ViewBag.CategoryId = categoryId;
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.RedirectUrl = Url.Action("Index", "Authentication");
                return PartialView("_Unauthorized");
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }
            finally
            {
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions( mLister.Pagination.PageSize );
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
            }

            return View("_Items", mLister);
        }
    }
}