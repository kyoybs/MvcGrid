using DevTool.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTool.Models
{
    public class SelectFieldsModel
    {
        public int CategoryId { get; set; }

        public string MainTable { get; set; }
        
        public List<DevFieldInfo> Fields { get; set; }
    }
}