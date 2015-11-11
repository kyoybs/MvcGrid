using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
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
    }
 
}