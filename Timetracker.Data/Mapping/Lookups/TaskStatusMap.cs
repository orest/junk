using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Data.Mapping.Lookups
{
    public class TaskStatusMap : EntityTypeConfiguration<TaskStatus>
    {
        public TaskStatusMap()
        {
            HasKey(t => t.Code);            
            
            Property(t => t.Description).HasMaxLength(50);            
            Property(t => t.Code).HasColumnName("TaskStatusCd").HasMaxLength(5);
            ToTable("Look_TaskStatus");
            Ignore(t => t.Id);
        }
    }
}
