using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Synvata.ORM
{
    internal class InsertSql
    {
        private string TableName { get; set; }

        private FieldInfo PkField { get; set; }

        private List<FieldInfo> InsertingFields { get; set; } = new List<FieldInfo>();

        public DynamicParameters Parameters { get; set; } = new DynamicParameters();

        public string GetInsertSql()
        {
            StringBuilder sql = new StringBuilder(InsertingFields.Count * 15);
            sql.AppendLine($"INSERT INTO {TableName} (");
            sql.AppendLine(string.Join(", ", InsertingFields.Select(field=> $"{field.FieldName}")) );
            sql.AppendLine(") VALUES ( ");
            sql.AppendLine(string.Join(", ", InsertingFields.Select(field => $"@{field.FieldName}")));
            sql.AppendLine(")  ; ");
            sql.AppendLine($" SELECT IDENT_CURRENT('{TableName}'); ");
            return sql.ToString();
        }

        private PropertyInfo _PkPropertyInfo  ;

        private object _Entity;

        public static InsertSql ParseEntity(object entity)
        { 
            InsertSql isql = new InsertSql();
            isql._Entity = entity;

            var type = entity.GetType();
            TableAttribute ta = type.GetCustomAttribute<TableAttribute>();
            isql.TableName = ta == null ? type.Name : ta.Name;

            var props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttribute<KeyAttribute>(true) == null)
                {
                    if (prop.GetCustomAttribute<NotMappedAttribute>(true) != null)
                        continue;
                    isql.InsertingFields.Add(FieldInfo.Parse(prop));
                    isql.Parameters.Add($"@{prop.Name}", prop.GetValue(entity));
                }
                else
                {
                    isql.PkField = FieldInfo.Parse(prop);
                    isql._PkPropertyInfo = prop;  
                    //isql.Parameters.Add($"@{prop.Name}", prop.GetValue(entity), direction: ParameterDirection.Output);
                }
            }

            return isql;
        }

        public int SetId(int id)
        {
            if (_PkPropertyInfo == null)
                return 0; 
            _PkPropertyInfo.SetValue(_Entity, id);
            return id;
        }

    }
}
