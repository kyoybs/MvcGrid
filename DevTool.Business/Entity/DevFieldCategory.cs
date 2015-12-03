using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business.Entity
{
    public class DevFieldCategory
    {
        [Key]
        public int FieldCategoryId { get; set; }

        public int FieldId { get; set; }

        public int CategoryId { get; set; }

    }
}
