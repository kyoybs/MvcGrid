using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Synvata.ORM
{
    internal class UpdateSql
    {
        private string TableName { get; set; }

        private FieldInfo PkField { get; set; }

        private List<FieldInfo> UpatingFields { get; set; } = new List<FieldInfo>(); 

        public string GetUpdateSql()
        {
            StringBuilder sql = new StringBuilder(UpatingFields.Count * 15);
            sql.AppendLine($"UPDATE {TableName} SET ");
            sql.AppendLine(string.Join(", ", UpatingFields.Select(field=> $"{field.FieldName}=@{field.PropertyName}")) );
            sql.AppendLine($"WHERE {PkField.FieldName}=@{PkField.PropertyName}");
            return sql.ToString();
        }
         
        public static UpdateSql ParseEntity(object entity)
        {
            UpdateSql usql = new UpdateSql();

            var type = entity.GetType();
            TableAttribute ta = type.GetCustomAttribute<TableAttribute>();
            usql.TableName = ta == null ? type.Name : ta.Name;

            var props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttribute<KeyAttribute>(true) == null)
                {
                    if (prop.GetCustomAttribute<NotMappedAttribute>(true) != null)
                        continue;
                    usql.UpatingFields.Add(FieldInfo.Parse(prop));
                }
                else
                {
                    usql.PkField = FieldInfo.Parse(prop);
                }
            }

            return usql;
        }

    }
}
