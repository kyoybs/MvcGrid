using MvcGrid.Business;
using MvcGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcGrid.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = TestBusiness.GetGridModel(null,true);
            model.Id = "Test";
            //model.JsSortFunction = "sortGrid"
            return View(model);
        }

        public ActionResult GetGrid(string orderField , bool isAsc)
        {
            var model = TestBusiness.GetGridModel(orderField, isAsc);
            model.Id = "Test";
            //model.JsSortFunction = "sortGrid"
            return PartialView("_Grid", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
         
    }
}