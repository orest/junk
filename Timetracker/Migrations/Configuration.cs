using System.Data.Entity.Migrations;
using System.Linq;
using Timetracker.Data;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TimeTrakerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MyProjects.Data.TimeTrakerContext";
        }

        protected override void Seed(TimeTrakerContext context)
        {

            if (!context.ProjectStatuses.Any())
            {
                context.ProjectStatuses.Add(new ProjectStatus { Id = 1, Description = "Active" });
                context.ProjectStatuses.Add(new ProjectStatus { Id = 2, Description = "Completed" });
            }

            context.Frequency.AddOrUpdate(
                p => p.Code,
                new Frequency { Code = "MNTH", Description = "Monthly" },
                new Frequency { Code = "WEEK", Description = "Weekly" });

            context.TimesheetCodes.AddOrUpdate(
                p => p.Code,
                new TimesheetCode() { Code = "ELNC", Description = "Elance " },
                new TimesheetCode() { Code = "INVCE", Description = "Invoice " },
                new TimesheetCode() { Code = "UPWRK", Description = "Upwork Former oDesk " }
                );


        }
    }
}
