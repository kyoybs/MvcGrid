using Newtonsoft.Json;
using Synvata.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTool.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            if(filterContext.Exception is SavableException)
            {
                filterContext.Result = Content(filterContext.Exception.Message);
                filterContext.ExceptionHandled = true;
                return;
            }
            base.OnException(filterContext);
        }
    }
}