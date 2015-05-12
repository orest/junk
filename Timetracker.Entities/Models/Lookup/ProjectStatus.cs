using System.Collections.Generic;
using Timetracker.Entities.Models.Base;

namespace Timetracker.Entities.Models.Lookup
{
    public class ProjectStatus : LookupBase
    {
        public ProjectStatus()
        {
            Projects = new List<Project>();
        }

        public ICollection<Project> Projects { get; set; }
    }
}
