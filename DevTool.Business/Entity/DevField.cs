using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business.Entity
{
    public class DevFieldInfo
    {
        [Key]
        public int FieldId { get; set; }

        public string TableName { get; set; }

        public string FieldName { get; set; }

        public string FieldLabel { get; set; }

        public bool Deleted { get; set; }


    }
}
