using DevTool.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DevTool.Business
{
    public class DevBusiness
    {
        public static List<DevFieldInfo> SearchFields(DevFieldInfo model , int excludedCategoryId=0)
        {
            StringBuilder sb = new StringBuilder( @"SELECT top 100 * FROM DevFieldInfo WHERE Deleted<>1 ");
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(model.TableName))
            {
                filters.Add(  "TableName like @TableName");
                model.TableName = "%" + model.TableName + "%";
            }

            if (!string.IsNullOrEmpty(model.FieldName))
            {
                filters.Add( "FieldName like @FieldName");
                model.FieldName = "%" + model.FieldName + "%";
            }

            if (!string.IsNullOrEmpty(model.FieldLabel))
            {
                filters.Add(  "FieldLabel like @FieldLabel");
                model.FieldLabel = "%" + model.FieldLabel + "%";
            }

            DynamicParameters dps = new DynamicParameters();
            dps.AddDynamicParams(model);

            if (excludedCategoryId > 0)
            {
                filters.Add("FieldId NOT IN (SELECT FieldId FROM DevFieldCategory WHERE CategoryId=@ExCategoryId) ");
                dps.Add("ExCategoryId", excludedCategoryId); 
            }

            if(filters.Count > 0)
            {
                sb.Append(" AND ");
                sb.Append(string.Join(" AND " , filters));
            }

            using (var db = DbHelper.Create())
            {
                return db.Query<DevFieldInfo>(sb.ToString(), dps ).ToList();
            }
        }

        public static void AddFieldToCategory(DevFieldInfo fieldInfo, int categoryId)
        {
            DevFieldCategory fc = new DevFieldCategory { CategoryId = categoryId, FieldId = fieldInfo.FieldId };
            using (var db = DbHelper.Create())
            {
                db.InsertEntity(fc);
            }
        }

       
        public static List<DevFieldInfo> GetCategoryFields(int categoryId)
        {
            string sql = @"SELECT F.* FROM DevFieldInfo f 
JOIN DevFieldCategory fc ON f.FieldId = fc.FieldId WHERE fc.CategoryId=@CategoryId";
            using (var db = DbHelper.Create())
            {
                return db.Query<DevFieldInfo>(sql, new { CategoryId = categoryId }).ToList();
            }
        }
    }
}
