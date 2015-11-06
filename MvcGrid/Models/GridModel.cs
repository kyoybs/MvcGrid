using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MvcGrid.Models
{
    public class GridModel
    {
        private string p_Id;

        public string Id
        {
            get
            {
                if (p_Id == null)
                    p_Id = Guid.NewGuid().ToString();
                return p_Id;
            }
            set
            {
                p_Id = value;
            }
        }
         
        public DataTable Data { get; set; }

        public List<GridField> Fields { get; set; } = new List<GridField>();

        private string p_SortField;

        public string SortField
        {
            get
            {
                if (p_SortField==null && Fields.Count >0)
                {
                    p_SortField = Fields.FirstOrDefault().FieldName;
                }
                return p_SortField;
            }
            set
            {
                p_SortField = value;
            }
        }

        public bool? IsAsc { get; set; }

        private string p_KeyField;

        public string KeyField
        {
            get
            {
                if (p_KeyField == null && Fields.Count > 0)
                {
                    p_KeyField = Fields.FirstOrDefault().FieldName;
                }
                return p_KeyField;
            }
            set
            {
                p_KeyField = value;
            }
        }

        /// <summary>
        /// a js function name , like: function sortGrid(string sortByField, bool sortByAsc);
        /// If there are more than 1 grid on a page, please assign different sort function for each grid.
        /// </summary>
        public string JsSortFunction
        {
            get
            {
                if (p_JsSortFunction == null)
                    p_JsSortFunction = "sortGrid";// + this.Id;
                return p_JsSortFunction;
            }
            set
            {
                p_JsSortFunction = value;
            }
        }

        private string p_JsSortFunction;
    }

    public class GridField
    {
        #region Serialization properties
        public string FieldName { get; set; }

        public string FieldTitle { get; set; }

        public string FieldWidth { get; set; }

        public bool Visible { get; set; } = true;

        public int Index { get; set; }

        public string FormatString { get; set; } = "";

        public bool Sortable { get; set; }
          
        #endregion Serialization properties

        #region Helper methods

        public string Format(object value)
        {
            return string.Format("{0}", value);
        }

        public string GetOrderChar(GridModel grd)
        {
            if (IsCurrentSortField(grd))
            {
                return grd.IsAsc == true ? "&uarr;" : "&darr;";
            }
            return "";
        }

        private bool IsCurrentSortField(GridModel grd)
        {
            return this.FieldName.Equals(grd.SortField, StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetHeaderAttrs(GridModel grd)
        { 
            Dictionary<string, string> attrs = new Dictionary<string, string>();
            attrs.Add("data-field", this.FieldName); 
            attrs.Add("data-asc", grd.IsAsc==true && this.IsCurrentSortField(grd) ? "1" : "0");

            if (this.Sortable)
            {
                attrs.Add("onclick", "_Grid_OrderBy(this);");
                attrs.Add("class", "order");
            }
             
            return string.Join(" ", attrs.Select(attr => $"{attr.Key}=\"{attr.Value}\"").ToArray());
        }

        public string GetDataAttrs()
        {
            Dictionary<string, string> attrs = new Dictionary<string, string>();
            attrs.Add("data-field", this.FieldName);

            return string.Join(" ", attrs.Select(attr => $"{attr.Key}={attr.Value}").ToArray());
        }

        #endregion Helper methods
    }
}