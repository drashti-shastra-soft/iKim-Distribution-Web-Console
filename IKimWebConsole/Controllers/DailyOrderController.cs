using IKimWebConsole.Filter;
using IKimWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;

namespace IKimWebConsole.Controllers
{
    [Authentication]
    public class DailyOrderController : Controller
    {
        private readonly IDailyOrderService _service;
        private readonly IStoreService _Storeservice;

        public DailyOrderController(IDailyOrderService service, IStoreService storeservice)
        {
            _service = service;
            _Storeservice = storeservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Daily Orders

        [HttpPost]
        public JsonResult DailyOrders(Domain.DailyOrder mDailyOrders)
        { 
            try 
            { 
                var mDailyOrder = _service.DailyOrdersAsync(mDailyOrders).GetAwaiter().GetResult(); 

                return Json(new { IsSuccess = true, Data = mDailyOrder }, JsonRequestBehavior.AllowGet);
            } 
            catch (Exception ex) 
            { 
                return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet); 
            }
        }
        #endregion

        #region Download PDF
        public ActionResult DownloadPdf(DateTime fromDate, DateTime toDate)
        {
            var dailyOrders = _service.DailyOrdersAsync(new Domain.DailyOrder { FromDate = fromDate, ToDate = toDate }).GetAwaiter().GetResult();

            var base64 = _service.GetPdfAsync(dailyOrders).GetAwaiter().GetResult();

            return Json(new
            {
                StatusCode = "Ok", Base64String = base64
            });
        }
        
        #endregion
    }
}