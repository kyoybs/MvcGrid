using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Dynamic;

namespace Synvata.ORM
{
    public class DbAgent :IDisposable
    { 
        private DbConnection _Connection;

        private DbProviderFactory _Factory;

        private DbAgent() { }

        public static DbAgent Create(string connectionName)
        {
            DbAgent db = new DbAgent();
            var config = ConfigurationManager.ConnectionStrings[connectionName];
            string providerName = config.ProviderName;
            if(string.IsNullOrEmpty(providerName))
                providerName = "System.Data.SqlClient";
            db._Factory = DbProviderFactories.GetFactory(providerName);
            db._Connection = db._Factory.CreateConnection();
            db._Connection.ConnectionString = config.ConnectionString;
            return db;
        }

        public void Dispose()
        {
            if (_Connection != null && _Connection.State != System.Data.ConnectionState.Closed)
                _Connection.Close();
            _Connection = null;
        }

        private DynamicParameters _Parameters; 

        public DbAgent AddParam(string name, object value, DbType? dbType, ParameterDirection? direction, int? size)
        {
            if (_Parameters == null)
                _Parameters = new DynamicParameters();
            _Parameters.Add(name, value, dbType, direction, size);
            return this;
        }
        
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? timeout=null, CommandType? commandType=null )
        {
            if (param == null)
                param = _Parameters;
            var list = await _Connection.QueryAsync<T>(sql, param, commandTimeout: timeout, commandType: commandType);
            if(_Parameters != null)
                _Parameters = null;
            return list;
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, int? timeout = null, CommandType? commandType = null)
        {
            if (param == null)
                param = _Parameters;
            var list = _Connection.Query<T>(sql, param, commandTimeout: timeout, commandType: commandType);
            if (_Parameters != null)
                _Parameters = null;
            return list;
        }

        public async Task ExecuteAsync(string sql, object param = null, int? timeout = null, CommandType? commandType = null)
        {
            await _Connection.ExecuteAsync(sql, param, commandTimeout: timeout, commandType: commandType);
        }

        public void Execute(string sql, object param = null, int? timeout = null, CommandType? commandType = null)
        {
            _Connection.Execute(sql, param, commandTimeout: timeout, commandType: commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, int? timeout = null, CommandType? commandType = null)
        {
            return _Connection.ExecuteScalar<T>(sql, param, commandTimeout: timeout, commandType: commandType) ;
        }
         
        public async Task UpdateEntityAsync(object entity, int? timeout = null)
        {
            UpdateSql sql = UpdateSql.ParseEntity(entity);
            await ExecuteAsync(sql.GetUpdateSql(), entity, timeout: timeout);
        }

        public void UpdateEntity(object entity, int? timeout = null)
        {
            UpdateSql sql = UpdateSql.ParseEntity(entity);
            Execute(sql.GetUpdateSql(), entity, timeout: timeout);
        }
         

        public void InsertEntity(object entity, int? timeout = null)
        {
            InsertSql sql = InsertSql.ParseEntity(entity);
            int id = ExecuteScalar<int>(sql.GetInsertSql(), entity, timeout: timeout);
            sql.SetId(id);
        }
    }
}
