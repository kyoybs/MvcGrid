using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcGrid.Models
{
    public class ViewBuilderIndexModel
    {
        public string CategoryName { get; set; }

        public List<string> Categories { get; set; }

        public DataTable FieldsData { get; set; }
    }
}