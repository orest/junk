using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models;

namespace Timetracker.Data.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            HasKey(t => t.ProjectId);


            // Relationships
            HasRequired(t => t.Client)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.ClientId);
            HasRequired(t => t.ProjectStatus)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.ProjectStatusId);

        }
    }
}
