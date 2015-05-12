using Timetracker.Data;
using Timetracker.Entities.Models.Lookup;

namespace MyProjects.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            context.ProjectStatuses.AddOrUpdate(
                p => p.Id,
                new ProjectStatus { Id = 1, Description = "Active" },
                new ProjectStatus { Id = 2, Description = "Completed" });

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
