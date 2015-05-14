using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models;

namespace Timetracker.Data.Mapping
{
    public class TaskMap : EntityTypeConfiguration<Task>
    {
        public TaskMap()
        {
            // Primary Key
            HasKey(t => t.TaskId);
            Property(t => t.StatusCd).HasMaxLength(5);
            
            // Relationships
            HasRequired(t => t.Project)
                .WithMany(t => t.Tasks)
                .HasForeignKey(d => d.ProjectId);
            
        }
    }
}
