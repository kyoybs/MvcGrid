using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTool.Models
{
    public class TreeModel<T>
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public bool Selected { get; set; }

        public T Entity { get; set; }

        public List<TreeModel<T>> Children { get; set; } = new List<TreeModel<T>>();
    }
}