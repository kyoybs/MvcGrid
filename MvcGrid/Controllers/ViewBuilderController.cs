using MvcGrid.Business;
using MvcGrid.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace MvcGrid.Controllers
{
    public class ViewBuilderController : Controller
    {
        // GET: ViewBuilder
        public ActionResult Index()
        {
            ViewBuilderIndexModel model = new ViewBuilderIndexModel();
            model.Categories = DevBiz.GetCategories();
            return View(model);
        }

        public ActionResult SearchFields()
        {
            SearchFieldsModel model = new SearchFieldsModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchFields(SearchFieldsModel model)
        {
            model.GridModel.Editable = true;
            model.GridModel.Addable = true;
            model.GridModel.Duplicatable = true;
            model.GridModel.JsDuplicateFunction = "duplicateField";
            model.GridModel.UrlUpdateField = Url.Content("~/ViewBuilder/UpdateField");
            DevBiz.SearchFields(model);
             
            return View(model);
        }

        [HttpPost]
        public string UpdateField(string fieldName, string fieldValue, int dataId)
        {
            //return fieldName + " -- " + fieldValue + " -- " + dataId;
            string sql = $"UPDATE DevFieldInfo SET {fieldName}=@FieldValue WHERE FieldId=@FieldId";
            using (var db = DbHelper.CreatConnect())
            {
                db.Execute(sql, new { FieldValue = fieldValue, FieldId=dataId });
            }
            return "";
        }

        [HttpPost]
        public string DuplicateField(int fieldId)
        {
            DevBiz.DuplicateField(fieldId);
            return "";
        }

        [HttpPost]
        public string Generate(SearchFieldsModel model)
        {
            string type = Request.QueryString["type"];
            string code = DevBiz.Generate(model, type);
            return code;
        }

    }
}