using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models;

namespace Timetracker.Data.Mapping
{
    public class WorkLogMap : EntityTypeConfiguration<WorkLog>
    {
        public WorkLogMap()
        {
            HasKey(t => t.WorkLogId);
            HasRequired(t => t.Project)
                           .WithMany(t => t.TimeSheet)
                           .HasForeignKey(d => d.ProjectId);

        }
    }
}
