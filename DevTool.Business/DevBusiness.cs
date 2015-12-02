using DevTool.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business
{
    public class DevBusiness
    {
        public static List<DevFieldInfo> Search(DevFieldInfo model)
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

            if(filters.Count > 0)
            {
                sb.Append(" AND ");
                sb.Append(string.Join(" AND " , filters));
            }

            using (var db = DbHelper.Create())
            {
                return db.Query<DevFieldInfo>(sb.ToString(), model).ToList();
            }
        }
    }
}
