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
        public string UpdateCategoryName(DevCategory category)
        {
            DevCategoryBusiness.UpdateCategoryName(category);
            return "";
        }

        [HttpPost]
        public string DeleteCategory(DevCategory category)
        {
            category.Deleted = true;
            DevCategoryBusiness.DeleteCategory(category);
            return "";
        }

        public ActionResult SelectFields(int categoryId, string mainTable)
        {  
            SelectFieldsModel model = new SelectFieldsModel { CategoryId = categoryId, MainTable = mainTable };
            DevFieldInfo field = new DevFieldInfo { TableName = mainTable };
            model.Fields = DevBusiness.SearchFields(field, categoryId, false);

            return View(model);
        }

        [HttpPost]
        public JsonResult SearchAllFields(int excludedCategoryId, bool fuzzy, DevFieldInfo field)
        { 
            var fields = DevBusiness.SearchFields(field , excludedCategoryId, fuzzy);
            return Json(fields); 
        }

        [HttpPost]
        public void AddFieldToCategory(int categoryId, DevFieldInfo field)
        {
            DevBusiness.AddFieldToCategory(field, categoryId);
        }

        [HttpPost]
        public JsonResult GetCategoryFields(int categoryId)
        {
            var fields = DevBusiness.GetCategoryFields(categoryId);
            return Json(fields);
        }
    }
}
