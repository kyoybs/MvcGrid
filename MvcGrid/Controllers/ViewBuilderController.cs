﻿using MvcGrid.Business;
using MvcGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcGrid.Controllers
{
    public class ViewBuilderController : Controller
    {
        // GET: ViewBuilder
        public ActionResult Index()
        {
            ViewBuilderIndexModel model = new ViewBuilderIndexModel();
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
            DevBiz.SearchFields(model);
            return View(model);
        }
    }
}