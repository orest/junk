using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Data.Mapping.Lookups
{
    public class FrequencyMap : EntityTypeConfiguration<Frequency>
    {
        public FrequencyMap()
        {            
            HasKey(t => t.Code);
            Property(t => t.Description).IsRequired().HasMaxLength(250);
            ToTable("Look_Frequency");
            Ignore(t => t.Id);
        }
    }
}
