using DevTool.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTool.Models
{
    public class HomeIndexModel
    {
        public List<DevControlType> DevControlTypes { get; set; }

        public List<KeyValuePair<int,string>> DataTypes { get; set; }
    }
}