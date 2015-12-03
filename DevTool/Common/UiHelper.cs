using DevTool.Business;
using DevTool.Business.Entity;
using DevTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DevTool.Common
{
    public class UiHelper
    {
        public static TreeModel<DevCategory> CategoryTree()
        {
            TreeModel<DevCategory> tree = new TreeModel<DevCategory>();
            tree.Name = "Categories";
            tree.Id = 1;

            var categories = DevCategoryBusiness.GetCategories();

            var root = categories.FirstOrDefault(c => c.ParentId == null || c.ParentId == 0);
            if (root != null)
            {
                tree.Name = root.CategoryName;
                tree.Id = root.CategoryId;
                tree.Entity = root;
            }
             
            GetChildCategories(tree, categories );

            return tree;
        }

        private static void GetChildCategories(TreeModel<DevCategory> tree, List<DevCategory> categories)
        {
            foreach (var item in categories.Where(c => c.ParentId == tree.Id))
            {
                var node = new TreeModel<DevCategory> { Id = item.CategoryId, Name = item.CategoryName, Entity = item };
                tree.Children.Add(node);
                GetChildCategories(node, categories);
            }
        }

        public static string ToJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj );
        }
    }
}