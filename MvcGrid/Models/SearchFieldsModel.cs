using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcGrid.Models
{
    public class SearchFieldsModel
    {
        public string TableName { get; set; }

        public string CategoryName { get; set; }

        public string FieldName { get; set; }

        public GridModel GridModel { get; set; }
    }
}