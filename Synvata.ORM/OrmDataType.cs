using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace Synvata.ORM
{
    public static class TypeExtension
    {
        public static DbType ToDbType(this Type csType)
        {
            DbType dbt;
            try
            {
                dbt = (DbType)Enum.Parse(typeof(DbType), csType.Name);
            }
            catch
            {
                dbt = DbType.Object;
            }
            return dbt;
        }
    }
}
