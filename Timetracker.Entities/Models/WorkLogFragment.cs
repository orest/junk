using System;

namespace Timetracker.Entities.Models
{
    public class WorkLogFragment
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ElapsedMinutes { get; set; }
        public int WorkLogId { get; set; }
        public WorkLog WorkLog { get; set; }
    }
}
