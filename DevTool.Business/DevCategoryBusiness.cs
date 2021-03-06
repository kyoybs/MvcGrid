﻿using DevTool.Business.Entity;
using Synvata.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business
{
    public class DevCategoryBusiness
    {
        public  static List<DevCategory> GetCategories()
        {
            using (var db = DbHelper.Create())
            {
                return db.Query<DevCategory>("SELECT * FROM dbo.DevCategory WHERE Deleted<>1").ToList();
            }
        }
         
        public static void UpdateCategory(DevCategory category)
        {
            using (var db = DbHelper.Create())
            {
                db.UpdateEntity(category);
            }
        }

        public static void InsertCategory(DevCategory category)
        {
            using (var db = DbHelper.Create())
            {
                db.InsertEntity(category);
            }
        }

        public static void UpdateCategoryName(DevCategory category)
        {
            string sql = @"UPDATE [dbo].[DevCategory] SET CategoryName=@CategoryName, MainTable=@MainTable WHERE CategoryId=@CategoryId";
            using (var db = DbHelper.Create())
            {
                db.Execute(sql, category);
            }
        }

        public static void DeleteCategory(DevCategory category)
        {
            string sql = @"UPDATE[dbo].[DevCategory] SET Deleted = 1 WHERE CategoryId = @CategoryId";
            using (var db = DbHelper.Create())
            {
                db.Execute(sql, category);
            }
        }
    }
}
