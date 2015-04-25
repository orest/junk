using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace MyProjects.Models
{
    public class WorkLog
    {
        public WorkLog()
        {
            Tasks = new List<Task>();
        }
        public int WorkLogId { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ElapsedMinutes { get; set; }
        public int WeekId { get; set; }
        public string Notes { get; set; }
        public List<Task> Tasks { get; set; }
        public Project Project { get; set; }
    }
}
