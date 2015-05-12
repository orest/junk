using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Timetracker.Entities.Models
{
    public class WorkLog
    {
        public WorkLog()
        {
            Tasks = new List<Task>();
        }
        public int WorkLogId { get; set; }
        public int ProjectId { get; set; }

       [Display(Name = "Start Date")]
       [DisplayFormat(DataFormatString = "{0:MMM dd HH:mm}")]
        public DateTime StartDate { get; set; }

       [Display(Name = "End Date")]
       [DisplayFormat(DataFormatString = "{0:MMM dd HH:mm}")]
        public DateTime? EndDate { get; set; }
        public int ElapsedMinutes { get; set; }
        public int WeekId { get; set; }
        public string Notes { get; set; }
        public List<Task> Tasks { get; set; }
        public Project Project { get; set; }
    }
}
