using System.Data.Entity;
using Timetracker.Data.Mapping;
using Timetracker.Data.Mapping.Lookups;
using Timetracker.Entities.Models;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Data
{
    public class TimeTrakerContext : DbContext
    {
        public TimeTrakerContext()
            : base("name=DefaultConnection")
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        public DbSet<WorkLogFragment> WorkLogFragments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<TimesheetCode> TimesheetCodes { get; set; }
        public DbSet<Frequency> Frequency { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new TaskMap());
            modelBuilder.Configurations.Add(new WorkLogMap());

            modelBuilder.Configurations.Add(new ProjectStatusMap());
            modelBuilder.Configurations.Add(new TimesheetCodeMap());
            modelBuilder.Configurations.Add(new TaskStatusMap());
            modelBuilder.Configurations.Add(new FrequencyMap());
        }
    }
}