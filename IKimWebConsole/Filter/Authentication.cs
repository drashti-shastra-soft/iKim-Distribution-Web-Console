using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IKimWebConsole.Filter
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var loginUser = Infrastructure.HttpContextHelper.LoginUser;

            if (loginUser == null)
            {
                var returnUrl = context.HttpContext?.Request?.RawUrl ?? "/";

                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Authentication" },
                    { "action", "Index" },
                    { "returnUrl", returnUrl }
                });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}