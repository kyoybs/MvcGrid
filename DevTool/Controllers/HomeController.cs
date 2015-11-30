using DevTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTool.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
             
            return View();
        }


        public JsonResult GetCategories()
        {
            TreeModel tm = new TreeModel();
            tm.Name = "Categories";
            TreeModel tm1 = new TreeModel();
            tm1.Name = "Ctg 1";
            tm.Children.Add(tm1);

            TreeModel tm2 = new TreeModel();
            tm2.Name = "Ctg 2";
            tm.Children.Add(tm2);

            TreeModel tm21 = new TreeModel();
            tm21.Name = "Ctg 21";
            tm2.Children.Add(tm21);

            var json = Json( tm);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}
