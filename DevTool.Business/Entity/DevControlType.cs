using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business.Entity
{
    public class DevControlType
    {
        public int ControlTypeId { get; set; }

        public string ControlTypeName { get; set; }

        public string ViewPattern { get; set; }

        public string EntityPattern { get; set; }

        public string KimlPattern { get; set; }

        public string HtmlPattern { get; set; }

        public string MvcPattern { get; set; } 
    }
}
