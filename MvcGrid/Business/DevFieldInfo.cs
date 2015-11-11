namespace MvcGrid.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DevFieldInfo")]
    public partial class DevFieldInfo
    {
        [Key]
        public int FieldId { get; set; }

        [Required]
        [StringLength(100)]
        public string TableName { get; set; }

        [Required]
        [StringLength(100)]
        public string FieldName { get; set; }

        [Required]
        [StringLength(500)]
        public string FieldLabel { get; set; }

        [StringLength(100)]
        public string EntityProperty { get; set; }

        public int FieldIndex { get; set; }

        public short IsPK { get; set; }

        public bool CanNull { get; set; }

        [Required]
        [StringLength(50)]
        public string DataType { get; set; }

        public int Length { get; set; }

        [StringLength(200)]
        public string CategoryName { get; set; }

        public int? ControlTypeId { get; set; }

        public int? ControlIndex { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
