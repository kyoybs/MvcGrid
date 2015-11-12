
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace System.Data
{
    public static class DbHelper
    { 
        public static SqlConnection CreatConnect()
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return new SqlConnection(strConn);
        }

        
        public static SqlBuilder CreateSql()
        {
            return SqlBuilder.Create();
        }
         
        public static void SaveThreadSql(string sql)
        { 
            LocalDataStoreSlot sqlSlot = Thread.GetNamedDataSlot("SqlLog");
            List<string> sqls = Thread.GetData(sqlSlot) as List<string>;
            if (sqls == null)
                sqls = new List<string>();
            sqls.Add(sql);
            Thread.SetData(sqlSlot, sqls);
        }

        public static string GetThreadSqls( )
        {
            LocalDataStoreSlot sqlSlot = Thread.GetNamedDataSlot("SqlLog");
            List<string> sqls = Thread.GetData(sqlSlot) as List<string>;
            if (sqls == null)
                return "";
            Thread.FreeNamedDataSlot("SqlLog");
            return Thread.CurrentThread.ManagedThreadId +" --- " + string.Join("\r\n", sqls).Replace("\r\n","<br/>");
        }

    }

    public class SqlBuilder:IDisposable
    {
        private SqlConnection _Connection { get; set; }

        public string Wrap(string value, string wrapBy = "%")
        {
            return string.IsNullOrEmpty(value) ? value : wrapBy + value + wrapBy;
        }

        private SqlBuilder()
        {

        }

        public static SqlBuilder Create()
        {
            var sb = new SqlBuilder();
            sb._Connection = DbHelper.CreatConnect();
            return sb;
        }



        public string SelectSql { get; set; }

        public string OrderBy { get; set; }

        private List<string> _FilterSqls = new List<string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName">tb.FieldName or FieldName</param>
        /// <param name="sqlFilterBy"></param>
        /// <returns></returns>
        public SqlBuilder AddFilter(string filterSql, string parmName=null,  object value=null)
        {
            if (!string.IsNullOrEmpty(parmName) && (value == null || value.ToString() == ""))
                return this;

            _FilterSqls.Add(filterSql);

            if (string.IsNullOrEmpty(parmName))
                return this;

            if (!parmName.StartsWith("@"))
                parmName = "@" + parmName;

            _Params.Add(parmName, value);
            return this;
        }

        private Dictionary<string, object> _Params = new Dictionary<string, object>();
         
        public void InitCommand(SqlCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.SelectSql);
            if (_FilterSqls.Count > 0)
                sb.Append(" WHERE ");
            sb.AppendLine(string.Join(" AND ", _FilterSqls));
            sb.AppendLine(this.OrderBy);

            command.CommandText = sb.ToString();
            sb.AppendLine();
            foreach (var parm in this._Params)
            {
                command.Parameters.AddWithValue(parm.Key, parm.Value??DBNull.Value);
                sb.AppendLine($"{parm.Key}: {parm.Value}");
            }
            
            DbHelper.SaveThreadSql(sb.ToString());
        }


        public DataTable QueryDataTable(  )
        {
            if (_Connection.State != ConnectionState.Open)
                _Connection.Open();

            var comm = _Connection.CreateCommand();
            this.InitCommand(comm);
            DbDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comm;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void Dispose()
        {
            if (_Connection != null && _Connection.State != ConnectionState.Closed)
                _Connection.Close();
        }
    }
 

}