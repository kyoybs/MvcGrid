using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business.Entity
{
    public class DevCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public int? ParentId { get; set; }

        public string CategoryName { get; set; }

        public string SelectSql { get; set; }

        public string Notes { get; set; }
    }
}
