
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Data
{
    public static class DbHelper
    { 
        public static SqlConnection Create()
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return new SqlConnection(strConn);
        }

        public static DataTable QueryDataTable(this SqlConnection connection , string sql, object param = null , CommandType commandType=CommandType.Text)
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            if(param != null)
            {
                
                var props = param.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var type = prop.PropertyType;
                    if (!type.IsValueType && !type.Equals(typeof(string)))
                        continue;
                    string parmName = "@" + prop.Name;
                    if(sql.IndexOf(parmName, StringComparison.CurrentCultureIgnoreCase ) >=0)
                    { 
                        parms.Add(parmName, prop.GetValue(param));
                    }
                }
            }

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var comm = connection.CreateCommand();
            comm.CommandText = sql;
            comm.CommandType = commandType;
            foreach (var parm in parms)
            {
                comm.Parameters.AddWithValue(parm.Key, parm.Value??DBNull.Value);
            }
            DbDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comm;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        
        public static SqlBuilder GetSqlBuilder()
        {
            return SqlBuilder.Create();
        }

        public static String WrapForLike(string value)
        {
            return value==null? null : "%" + value + "%";
        }
    }

    public class SqlBuilder
    {
        private SqlBuilder()
        {

        }

        public static SqlBuilder Create()
        {
            return new SqlBuilder();
        }

        public string SelectSql { get; set; }

        private List<string> _FilterSqls = new List<string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName">tb.FieldName or FieldName</param>
        /// <param name="sqlFilterBy"></param>
        /// <returns></returns>
        public SqlBuilder AddFilterField(string fieldName, string sqlFilterBy = "=")
        {
            _FilterSqls.Add($"(@{fieldName} is null OR {fieldName} {sqlFilterBy} @{fieldName})");
            return this;
        }

        public SqlBuilder AddFilterSql(string filterSql)
        {
            _FilterSqls.Add(filterSql);
            return this;
        }

        public string BuildSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.SelectSql);
            if (_FilterSqls.Count > 0)
                sb.Append(" WHERE ");
            sb.AppendLine(string.Join(" AND ", _FilterSqls));
            return sb.ToString();
        }
    }
 

}