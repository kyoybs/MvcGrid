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
            using (var sqlb = DbHelper.CreateSql())
            {
                sqlb.SelectSql = @"SELECT TOP 100 f.*, t.ControlTypeName FROM DevFieldInfo f LEFT JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
                sqlb.AddFilter("f.TableName LIKE @TableName", "@TableName", sqlb.Wrap(searchFieldsModel.TableName));
                sqlb.AddFilter("f.FieldName LIKE @FieldName", "@FieldName", sqlb.Wrap(searchFieldsModel.FieldName));
                sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));
                sqlb.OrderBy = " ORDER BY ControlIndex ASC ";

                searchFieldsModel.GridModel.Data = sqlb.QueryDataTable();
                searchFieldsModel.GridModel.AutoSetFields();
            } 
        }

        public static string Generate(SearchFieldsModel searchFieldsModel)
        {
            using (var sqlb = DbHelper.CreateSql())
            {
                sqlb.SelectSql = @"SELECT TOP 100 f.*, t.ControlTypeName,t.Pattern FROM DevFieldInfo f JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
                sqlb.AddFilter("f.TableName LIKE @TableName", "@TableName", sqlb.Wrap(searchFieldsModel.TableName));
                sqlb.AddFilter("f.FieldName LIKE @FieldName", "@FieldName", sqlb.Wrap(searchFieldsModel.FieldName));
                sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));
                sqlb.OrderBy = " ORDER BY ControlIndex ASC ";

                StringBuilder sb = new StringBuilder();

                var dtFields = sqlb.QueryDataTable();
                foreach (DataRow row in dtFields.Rows)
                {
                    sb.AppendLine(row["Pattern"]?.ToString()?.Replace("[PropertyName]", row["EntityProperty"]?.ToString()));
                }
                return sb.ToString();
            } 
        }

        public static string GenerateEntity(SearchFieldsModel searchFieldsModel)
        {
            using (var sqlb = DbHelper.CreateSql())
            {
                sqlb.SelectSql = @"SELECT TOP 100 f.*, t.ControlTypeName,t.Pattern,t.EntityPattern FROM DevFieldInfo f JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
                sqlb.AddFilter("f.TableName LIKE @TableName", "@TableName", sqlb.Wrap(searchFieldsModel.TableName));
                sqlb.AddFilter("f.FieldName LIKE @FieldName", "@FieldName", sqlb.Wrap(searchFieldsModel.FieldName));
                sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));
                sqlb.OrderBy = " ORDER BY ControlIndex ASC "; 

                StringBuilder sb = new StringBuilder();

                var dtFields = sqlb.QueryDataTable();
                foreach (DataRow row in dtFields.Rows)
                {
                    string str = row["EntityPattern"]?.ToString()?.Replace("[PropertyName]", row["EntityProperty"]?.ToString());
                    string type = GetCsTypeName(row["DataType"].ToString());
                    str = str?.Replace("[Type]", type);
                    sb.AppendLine(str);
                }
                return sb.ToString();
            } 
        }
         
        public static string GetCsTypeName(string sqlTypeName)
        {
            if (sqlTypeName.IndexOf("char") >= 0)
                return "string";
            if (sqlTypeName.IndexOf("date") >= 0)
                return "DateTime";
            if (sqlTypeName.IndexOf("bigint") >= 0)
                return "log";
            if (sqlTypeName.IndexOf("int") >= 0)
                return "int";
            if (sqlTypeName.IndexOf("bit") >= 0)
                return "bool";  
            return "float";
        }

        public static List<string> GetCategories()
        {
            using (var sql = DbHelper.CreateSql())
            {
                sql.SelectSql = "SELECT DISTINCT CategoryName FROM  dbo.DevFieldInfo WHERE CategoryName IS NOT NULL";
                var list = sql.QueryDataTable().AsEnumerable().Select(r => r.Field<string>("CategoryName")).ToList();
                return list;
            }
        }
    }
}