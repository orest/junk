using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Data.Mapping.Lookups
{
    public class TimesheetCodeMap : EntityTypeConfiguration<TimesheetCode>
    {
        public TimesheetCodeMap()
        {
            HasKey(t => t.Code);
            Property(t => t.Code).IsRequired().HasMaxLength(5);
            Property(t => t.Description).HasMaxLength(250);
            ToTable("Look_TimesheetCode");
            Property(t => t.Code).HasColumnName("TimesheetCd");
            Ignore(t => t.Id);
        }
    }
}
