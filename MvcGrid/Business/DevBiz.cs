using MvcGrid.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcGrid.Business
{
    public class DevBiz
    {
        public static void SearchFields(SearchFieldsModel searchFieldsModel)
        {
            var sqlb = DbHelper.GetSqlBuilder();

            sqlb.SelectSql = "SELECT * FROM DevFieldInfo ";
            sqlb.AddFilterField( "TableName", "like" );
            sqlb.AddFilterField( "FieldName", "like" );
            sqlb.AddFilterField( "CategoryName", "like" );

            searchFieldsModel.TableName = DbHelper.WrapForLike( searchFieldsModel.TableName);
            searchFieldsModel.FieldName = DbHelper.WrapForLike(searchFieldsModel.FieldName);
            searchFieldsModel.CategoryName = DbHelper.WrapForLike(searchFieldsModel.CategoryName);
            using (var conn = DbHelper.Create())
            {
                searchFieldsModel.GridModel.Data = conn.QueryDataTable(sqlb.BuildSql(), searchFieldsModel);
                searchFieldsModel.GridModel.AutoSetFields();
            }   
        }
    }
}