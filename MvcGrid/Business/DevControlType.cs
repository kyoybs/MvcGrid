namespace MvcGrid.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DevControlType")]
    public partial class DevControlType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ControlTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string ControlTypeName { get; set; }

        [Required]
        [StringLength(500)]
        public string Pattern { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
