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
                sqlb.AddFilter("f.FieldLabel LIKE @FieldLabel", "@FieldLabel", sqlb.Wrap(searchFieldsModel.FieldLabel));
                sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));
                sqlb.OrderBy = " ORDER BY ControlIndex ASC ";
                searchFieldsModel.GridModel.SortField = "ControlIndex";
                searchFieldsModel.GridModel.Data = sqlb.QueryDataTable();
                searchFieldsModel.GridModel.AutoSetFields();
            } 
        }
         
        public static string Generate(SearchFieldsModel searchFieldsModel, string generateType="")
        {
            using (var sqlb = DbHelper.CreateSql())
            {
                sqlb.SelectSql = @"SELECT TOP 100 f.*, t.ControlTypeName,t.Pattern,t.EntityPattern,t.KimlPattern, t.HtmlPattern, t.MvcPattern FROM DevFieldInfo f JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
                sqlb.AddFilter("f.TableName LIKE @TableName", "@TableName", sqlb.Wrap(searchFieldsModel.TableName));
                sqlb.AddFilter("f.FieldName LIKE @FieldName", "@FieldName", sqlb.Wrap(searchFieldsModel.FieldName));
                sqlb.AddFilter("f.CategoryName LIKE @CategoryName", "@CategoryName", sqlb.Wrap(searchFieldsModel.CategoryName));
                sqlb.AddFilter("f.FieldLabel LIKE @FieldLabel", "@FieldLabel", sqlb.Wrap(searchFieldsModel.FieldLabel));
                sqlb.OrderBy = " ORDER BY ControlIndex ASC "; 

               

                var dtFields = sqlb.QueryDataTable();
                if(generateType == "sql")
                {
                    return GenerateSql(  dtFields);
                }

                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in dtFields.Rows)
                {
                    string str = row[generateType+"Pattern"]?.ToString()?.Replace("[PropertyName]", row["EntityProperty"]?.ToString());
                    string type = GetCsTypeName(row["DataType"].ToString());
                    str = str?.Replace("[Type]", type);
                    str = str?.Replace("[FieldLabel]", row["FieldLabel"].ToString());
                    string nullable = "";
                    if (Convert.ToBoolean(row["CanNull"]) && !"string".Equals(type, StringComparison.CurrentCultureIgnoreCase))
                        nullable = "?";
                    str = str?.Replace("[Nullable]",nullable);
                    str = str?.Replace("[FieldName]", row["FieldName"].ToString());
                    sb.AppendLine(str);
                    sb.AppendLine();
                }
                return sb.ToString();
            } 
        }

        private static string GenerateSql( DataTable dtFields)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ");
            sb.AppendLine(string.Join(",", dtFields.AsEnumerable().Select(r => (string)r["FieldName"]).ToList().Where(f => !string.IsNullOrEmpty(f))));
            sb.Append("FROM ");
            sb.AppendLine(string.Join(",", dtFields.AsEnumerable().Select(r => (string)r["TableName"]).Distinct().ToList().Where(f => !string.IsNullOrEmpty(f))));
            return sb.ToString();
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
                sql.SelectSql = "SELECT DISTINCT CategoryName FROM  dbo.DevFieldInfo WHERE CategoryName IS NOT NULL ORDER BY CategoryName ";
                var list = sql.QueryDataTable().AsEnumerable().Select(r => r.Field<string>("CategoryName")).ToList();
                return list;
            }
        }

        public static void DuplicateField( int fieldId)
        {
            String sql = @"INSERT  INTO DevFieldInfo ( [TableName], [FieldName],
                            [FieldLabel], [EntityProperty],
                            [FieldIndex], [IsPK], [CanNull],
                            [DataType], [Length],
                            [CategoryName], [ControlTypeId],
                            [ControlIndex], [Notes],
                            [Deleted] )
        SELECT  [TableName], [FieldName], [FieldLabel],
                [EntityProperty], [FieldIndex], [IsPK],
                [CanNull], [DataType], [Length],
                [CategoryName], [ControlTypeId],
                [ControlIndex], [Notes], [Deleted]
        FROM    dbo.DevFieldInfo
        WHERE   FieldId = @FieldId";
            using (var db = DbHelper.CreatConnect())
            {
                db.Execute(sql, new { FieldId = fieldId  });
            }
        }

        public static GridModel GetField(int fieldId)
        {
            GridModel model = new GridModel();

            using (var sqlb = DbHelper.CreateSql())
            {
                sqlb.SelectSql = @"SELECT f.*, t.ControlTypeName FROM DevFieldInfo f LEFT JOIN dbo.DevControlType t ON f.ControlTypeId=t.ControlTypeId ";
                sqlb.AddFilter("f.FieldId = @FieldId", "@FieldId",  fieldId);
                model.Data = sqlb.QueryDataTable() ;
                model.AutoSetFields();
            }
            return model;
        }
    }
}