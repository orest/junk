using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Data.Mapping.Lookups
{
    public class ProjectStatusMap : EntityTypeConfiguration<ProjectStatus>
    {
        public ProjectStatusMap()
        {
            HasKey(t => t.Id);            
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.Description).HasMaxLength(50);
            ToTable("Look_ProjectStatus");
            Property(t => t.Id).HasColumnName("ProjectStatusId");
            Property(t => t.Description).HasColumnName("Description");
            Ignore(t => t.Code);
        }
    }
}
