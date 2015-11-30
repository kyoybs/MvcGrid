using DevTool.Business.Entity;
using DevTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTool.Common
{
    public class UiHelper
    {
        public static TreeModel CategoryTree()
        {
            //List<DevCategory> categories = DevCategoryBusiness.

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

            return tm;

            //TreeModel tree = new TreeModel();
            //tree.Name = "Categories";

            //foreach (var item in categories.Where(c=>c.ParentId == null || c.ParentId == 0 ))
            //{
            //    tree.Children.Add(item)
            //}
        }

        public static string ToJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj );
        }
    }
}