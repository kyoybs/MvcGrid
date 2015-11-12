using MvcGrid.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using System.Text;

namespace MvcGrid.Business
{
    public class DevBiz
    {
        public static void SearchFields(SearchFieldsModel searchFieldsModel)
        {
            var sqlb = DbHelper.GetSqlBuilder();

            sqlb.SelectSql = @"SELECT f.*, t.ControlTypeName FROM DevFieldInfo f JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
            sqlb.AddFilter( "f.TableName LIKE @TableName", "@TableName", sqlb.Wrap(searchFieldsModel.TableName));
            sqlb.AddFilter("f.FieldName LIKE @FieldName", "@FieldName", sqlb.Wrap(searchFieldsModel.FieldName));
            sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));
             
            using (var conn = DbHelper.Create())
            {
                searchFieldsModel.GridModel.Data = conn.QueryDataTable(sqlb );
                searchFieldsModel.GridModel.AutoSetFields();
            }   
        }

        public static string Generate(SearchFieldsModel searchFieldsModel)
        {
            var sqlb = DbHelper.GetSqlBuilder();

            sqlb.SelectSql = @"SELECT f.*, t.ControlTypeName,t.Pattern FROM DevFieldInfo f JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
            sqlb.AddFilter("f.TableName LIKE @TableName", "@TableName", sqlb.Wrap(searchFieldsModel.TableName));
            sqlb.AddFilter("f.FieldName LIKE @FieldName", "@FieldName", sqlb.Wrap(searchFieldsModel.FieldName));
            sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));

            using (var conn = DbHelper.Create())
            {
                StringBuilder sb = new StringBuilder();

                var dtFields = conn.QueryDataTable(sqlb); 
                foreach (DataRow row in dtFields.Rows)
                {
                    sb.AppendLine(row["Pattern"]?.ToString()?.Replace("[PropertyName]", row["EntityProperty"]?.ToString()));
                }
                return sb.ToString();
            }
        }
    }
}