using System.Data.Entity.ModelConfiguration;
using Timetracker.Entities.Models;

namespace Timetracker.Data.Mapping
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            // Primary Key
            HasKey(t => t.ClientId);

            // Properties
            Property(t => t.CompanyName).HasMaxLength(100);
            Property(t => t.ContactName).HasMaxLength(100);
            Property(t => t.Phone).HasMaxLength(20);
            Property(t => t.Skype).HasMaxLength(100);
            Property(t => t.Email).HasMaxLength(100);
            Property(t => t.TimesheetCode).IsRequired().HasMaxLength(5);
            Property(t => t.TimesheetFrequencyCode).IsRequired().HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Clients");
            this.Property(t => t.TimesheetCode).HasColumnName("TimesheetCd");
            this.Property(t => t.TimesheetFrequencyCode).HasColumnName("TimesheetFrequencyCd");


            //// Relationships
            //this.HasRequired(t => t.TimesheetCode)
            //    .WithMany(t => t.Clients)
            //    .HasForeignKey(d => d.TimesheetCd);
            //this.HasRequired(t => t.Look_Frequency)
            //    .WithMany(t => t.Clients)
            //    .HasForeignKey(d => d.TimesheetFrequencyCd);

        }
    }
}
