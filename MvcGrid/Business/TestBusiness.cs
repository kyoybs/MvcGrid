using MvcGrid.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcGrid.Business
{
    public class TestBusiness
    {
        public static GridModel GetGridModel(string sortField , bool isAsc)
        {
            GridModel gm = new GridModel();
            DataTable dt = new DataTable();
            dt.Columns.Add("TestId");
            dt.Columns.Add("TestName");
            dt.Columns.Add("TestPrice");

            DataRow row = dt.NewRow();
            row["TestId"] = 1;
            row["TestName"] = "NXXXX";
            row["TestPrice"] = 12.345;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["TestId"] = 2;
            row["TestName"] = "NXXXX222";
            row["TestPrice"] = 22.345;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["TestId"] = 3;
            row["TestName"] = "NXXXX333";
            row["TestPrice"] = 32.345;
            dt.Rows.Add(row);

            gm.Data = dt;

            gm.Fields.Add(new GridField { FieldName = "TestId", Index = 0, FieldTitle = "Test ID" , Sortable=true });
            gm.Fields.Add(new GridField { FieldName = "TestName", Index = 0, FieldTitle = "Test Name", Sortable = true });
            gm.Fields.Add(new GridField { FieldName = "TestPrice", Index = 0, FieldTitle = "Test Price"  });

            gm.SortField = sortField;
            gm.IsAsc = isAsc; 

            return gm;
        }
    }
}