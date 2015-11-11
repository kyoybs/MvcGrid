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
        public static DataTable SearchFields(SearchFieldsModel searchFieldsModel)
        {
            using (var conn = DbHelper.Create())
            {
                conn.QueryDataTable("")
            }   
        }
    }
}