using IKimWebConsole.Infrastructure.IService;
using IKimWebConsole.Filter;
using System.Web.Mvc;
using System;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _service;
        private readonly IDropdownService _dropdownService;

        public DashboardController(IDashboardService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            try
            {
                ViewBag.Stores = _dropdownService.GetStoreOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Vendors = _dropdownService.GetVendorsOptionsAsync().GetAwaiter().GetResult();
                ViewBag.Items = _dropdownService.GetRawMaterialOptionsAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        #region Sales Chart

        [HttpGet]
        public JsonResult SalesChart(int storeId)
        {
            try
            {
                var mData = _service.SalesChartAsync(storeId).GetAwaiter().GetResult();

                return Json(new { IsSuccess = true, Data = mData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Purchase Chart

        [HttpGet]
        public JsonResult PurchaseChart(int vendorId)
        {
            try
            {
                var mData = _service.PurchaseChartAsync(vendorId).GetAwaiter().GetResult();
                return Json(new { IsSuccess = true, Data = mData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Ingredient Consumption Report

        [HttpGet]
        public JsonResult IngredientConsumptionReport(int itemId)
        {
            try
            {
                var mData = _service.IngredientConsumptionReportAsync(itemId).GetAwaiter().GetResult();

                return Json(new { IsSuccess = true, Data = mData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
    }
}