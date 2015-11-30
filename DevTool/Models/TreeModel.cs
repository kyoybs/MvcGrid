using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTool.Models
{
    public class TreeModel
    {
        public string Name { get; set; }

        public List<TreeModel> Children { get; set; } = new List<TreeModel>();
    }
}