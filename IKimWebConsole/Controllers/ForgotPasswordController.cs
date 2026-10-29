using IKimWebConsole.Infrastructure.IService;
using System.Web.Mvc;
using System;

namespace IKimWebConsole.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IForgotPasswordService _service;

        public ForgotPasswordController(IForgotPasswordService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Domain.ForgotPassword());
        }

        [HttpPost]
        public ActionResult Index(Domain.ForgotPassword mForgotPassword)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.Initiate(mForgotPassword).GetAwaiter().GetResult();

                ViewBag.NotificationType = "success";
                ViewBag.NotificationMsg = "We have sent new password on your register email address!";
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return View(mForgotPassword);
        }
    }
}