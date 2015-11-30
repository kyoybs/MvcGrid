using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Synvata.ORM
{
    internal class FieldInfo
    {
        public string FieldName { get; set; }

        public string PropertyName { get; set; }
         
        public static FieldInfo Parse(PropertyInfo prop)
        {
            ColumnAttribute colAttr = prop.GetCustomAttribute<ColumnAttribute>(true);
            if (colAttr == null)
                return new FieldInfo { FieldName = prop.Name, PropertyName = prop.Name };

            return new FieldInfo { FieldName = colAttr.Name, PropertyName = prop.Name };
        }

    }
}
