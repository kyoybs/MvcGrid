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
        public JsonResult InsertCategory(DevCategory category)
        { 
            DevCategoryBusiness.InsertCategory(category);
            TreeModel<DevCategory> tm = new TreeModel<DevCategory>();
            tm.Id = category.CategoryId;
            tm.Name = category.CategoryName;
            tm.Entity = category;
            return Json(tm);
        }

        [HttpPost]
        public void UpdateCategoryName(DevCategory category)
        {
            DevCategoryBusiness.UpdateCategoryName(category);
        }

        [HttpPost]
        public void DeleteCategory(DevCategory category)
        {
            category.Deleted = true;
            DevCategoryBusiness.DeleteCategory(category);
        }

        public ActionResult SelectFields(int homeMarketingId)
        {
            return View();
        }

        [HttpPost]
        public JsonResult SearchAllFields(DevFieldInfo field)
        {
            var fields = DevBusiness.Search(field);
            return Json(fields); 
        }
    }
}
