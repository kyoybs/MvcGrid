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
            HomeIndexModel model = new HomeIndexModel();
            model.DevControlTypes = DevBusiness.GetControlTypes();
            ViewBag.Title = "Home Page"; 
            return View(model);
        }
        
        [HttpPost]
        public JsonResult InsertCategory(DevCategory category)
        { 
            DevCategoryBusiness.InsertCategory(category);
            return Json(category);
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

        [HttpPost]
        public void SaveCategoryField(int categoryId, DevFieldInfo field)
        {
            DevBusiness.SaveCategoryField(categoryId, field);
        }

        [HttpPost]
        public string Generate(int categoryId, string type)
        { 
            string code = DevBusiness.Generate(categoryId, type);
            return code;
        }

    }
}
