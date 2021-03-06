﻿using DevTool.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Synvata.ORM;

namespace DevTool.Business
{
    public class DevBusiness
    {
        public static List<DevFieldInfo> SearchFields(DevFieldInfo model, int excludedCategoryId = 0, bool like = true)
        {
            StringBuilder sb = new StringBuilder(@"SELECT top 200 * FROM DevFieldInfo WHERE Deleted<>1 ");
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(model.TableName))
            {
                filters.Add("TableName like @TableName");
                if (like)
                    model.TableName = "%" + model.TableName + "%";
            }

            if (!string.IsNullOrEmpty(model.FieldName))
            {
                filters.Add("FieldName like @FieldName");
                if (like)
                    model.FieldName = "%" + model.FieldName + "%";
            }

            if (!string.IsNullOrEmpty(model.FieldLabel))
            {
                filters.Add("FieldLabel like @FieldLabel");
                if (like)
                    model.FieldLabel = "%" + model.FieldLabel + "%";
            }

            DynamicParameters dps = new DynamicParameters();
            dps.AddDynamicParams(model);

            if (excludedCategoryId > 0)
            {
                filters.Add("FieldId NOT IN (SELECT FieldId FROM DevFieldCategory WHERE CategoryId=@ExCategoryId AND Deleted<>1) ");
                dps.Add("ExCategoryId", excludedCategoryId);
            }

            if (filters.Count > 0)
            {
                sb.Append(" AND ");
                sb.Append(string.Join(" AND ", filters));
            }

            using (var db = DbHelper.Create())
            {
                return db.Query<DevFieldInfo>(sb.ToString(), dps).ToList();
            }
        }

        public static void AddFieldToCategory(DevFieldInfo fieldInfo, int categoryId)
        {
            DevFieldCategory fc = new DevFieldCategory
            {
                CategoryId = categoryId,
                FieldId = fieldInfo.FieldId,
                ControlIndex = fieldInfo.ControlIndex
            };
            using (var db = DbHelper.Create())
            {
                db.InsertEntity(fc);
            }
        }


        public static List<DevFieldInfo> GetCategoryFields(int categoryId)
        {
            string sql = @"
SELECT F.*, fc.ControlIndex , ct.ControlTypeName FROM DevFieldInfo f 
JOIN DevFieldCategory fc ON f.FieldId = fc.FieldId 
LEFT JOIN [dbo].[DevControlType] ct ON f.ControlTypeId=ct.ControlTypeId
WHERE fc.CategoryId=@CategoryId AND fc.Deleted<>1 
ORDER BY fc.ControlIndex ";
            using (var db = DbHelper.Create())
            {
                return db.Query<DevFieldInfo>(sql, new { CategoryId = categoryId }).ToList();
            }
        }

        public static DevFieldInfo SaveCategoryField(int categoryId, DevFieldInfo field)
        {
            if (field.FieldId > 0)
            {
                string sql = @"UPDATE DevFieldInfo SET FieldLabel=@FieldLabel, EntityProperty=@EntityProperty, ControlTypeId=@ControlTypeId WHERE FieldId=@FieldId ; 
UPDATE DevFieldCategory SET ControlIndex=@ControlIndex WHERE  FieldId=@FieldId AND CategoryId=@CategoryId";
                using (var db = DbHelper.Create())
                {
                    db.Execute(sql, new
                    {
                        CategoryId = categoryId,
                        FieldId = field.FieldId,
                        EntityProperty = field.EntityProperty,
                        ControlTypeId = field.ControlTypeId,
                        FieldLabel = field.FieldLabel,
                        ControlIndex = field.ControlIndex
                    });
                }
            }
            else
            {
                string sql = @"SELECT * FROM DevFieldInfo 
WHERE EntityProperty=@EntityProperty AND FieldLabel=@FieldLabel";
                using (var db = DbHelper.Create())
                {
                    var fld = db.Query<DevFieldInfo>(sql, field).FirstOrDefault();
                    if (fld != null)
                    {
                        field = fld;
                    }
                    else
                    {
                        db.InsertEntity(field);
                    }

                    AddFieldToCategory(field, categoryId);
                }
            }
            return field;
        }

        public static List<DevControlType> GetControlTypes()
        {
            string sql = @"SELECT * FROM DevControlType";
            using (var db = DbHelper.Create())
            {
                return db.Query<DevControlType>(sql).ToList();
            }
        }

        public static void RemoveField(int categoryId, int fieldId)
        {
            string sql = @"UPDATE DevFieldCategory SET Deleted = 1 
WHERE CategoryId=@CategoryId AND FieldId = @FieldId ";
            using (var db = DbHelper.Create())
            {
                db.Execute(sql, new { categoryId = categoryId, FieldId = fieldId });
            }
        }

        public static string Generate(int categoryId, string generateType = "")
        {
            var fields = GetCategoryFields(categoryId);

            if ("sql".Equals(generateType, StringComparison.CurrentCultureIgnoreCase))
            {
                return GenerateSql(fields);
            }

            List<DynamicEntity<DevControlType>> types = GetControlTypes().Select(t => new DynamicEntity<DevControlType>(t)).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var field in fields)
            {
                var ctlType = types.FirstOrDefault(t => t.Entity.ControlTypeId == field.ControlTypeId);
                if (ctlType == null)
                {
                    throw new SavableException("Field: " + field.FieldName + " need control type. ");
                }
                string str = ctlType.GetProperty<string>(generateType + "Pattern")?.Replace("[PropertyName]", field.EntityProperty);
                string type = GetCsTypeName(field.DataType);
                str = str?.Replace("[Type]", type);
                str = str?.Replace("[FieldLabel]", field.FieldLabel);
                string nullable = "";
                if (Convert.ToBoolean(field.CanNull) && !"string".Equals(type, StringComparison.CurrentCultureIgnoreCase))
                    nullable = "?";
                str = str?.Replace("[Nullable]", nullable);
                str = str?.Replace("[FieldName]", field.FieldName);
                sb.AppendLine(str);
                sb.AppendLine();
            }
            return sb.ToString();

        }

        private static string GenerateSql(List<DevFieldInfo> fields)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ");
            sb.AppendLine(string.Join(",", fields.Select(r => r.FieldName).ToList().Where(f => !string.IsNullOrEmpty(f))));
            sb.Append("FROM ");
            sb.AppendLine(string.Join(",", fields.Select(r => r.TableName).Distinct().ToList().Where(f => !string.IsNullOrEmpty(f))));
            return sb.ToString();
        }

        public static string GetCsTypeName(string sqlTypeName)
        {
            if (string.IsNullOrEmpty(sqlTypeName) || sqlTypeName.IndexOf("char") >= 0)
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

        public static List<KeyValuePair<int, string>> GetDataTypes()
        {
            string sql = "SELECT system_type_id AS [Key], name AS [Value]  FROM sys.types";
            using (var db = DbHelper.Create())
            {
                return db.Query<KeyValuePair<int, string>>(sql).ToList();
            }
        }
    }
}
