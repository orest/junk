using System.Data.Entity.Migrations;
using System.Linq;
using Timetracker.Entities.Models.Lookup;

namespace Timetracker.Data.Migrations
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
                context.ProjectStatuses.Add(new ProjectStatus { Code = "ACTV", Description = "Active" });
                context.ProjectStatuses.Add(new ProjectStatus { Code = "CMPTD", Description = "Completed" });
            }


            if (!context.TaskStatuses.Any())
            {
                context.TaskStatuses.Add(new TaskStatus { Code = "TODO", Description = "ToDo" });
                context.TaskStatuses.Add(new TaskStatus { Code = "ACTV", Description = "Active" });
                context.TaskStatuses.Add(new TaskStatus { Code = "INPR", Description = "In Progress" });
                context.TaskStatuses.Add(new TaskStatus { Code = "CMPTD", Description = "Completed" });                
                
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
