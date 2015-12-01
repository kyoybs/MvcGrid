using DevTool.Business;
using DevTool.Business.Entity;
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
        
        [HttpPost]
        public void SaveCategory(DevCategory category)
        { 
            DevCategoryBusiness.UpdateCategory(category); 
        }

        [HttpPost]
        public void UpdateCategoryName(DevCategory category)
        {
            DevCategoryBusiness.UpdateCategoryName(category);
        }
    }
}
