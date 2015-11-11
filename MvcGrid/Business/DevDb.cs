namespace MvcGrid.Business
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DevDb : DbContext
    {
        public DevDb()
            : base("name=DevDb")
        {
        }

        public virtual DbSet<DevControlType> DevControlTypes { get; set; }
        public virtual DbSet<DevFieldInfo> DevFieldInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DevControlType>()
                .Property(e => e.ControlTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<DevControlType>()
                .Property(e => e.Pattern)
                .IsUnicode(false);

            modelBuilder.Entity<DevControlType>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.TableName)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.FieldName)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.FieldLabel)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.EntityProperty)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.DataType)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<DevFieldInfo>()
                .Property(e => e.Notes)
                .IsUnicode(false);
        }
    }
}
