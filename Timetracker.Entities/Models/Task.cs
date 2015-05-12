namespace Timetracker.Entities.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public int StatusId { get; set; }
        public int ProjectId { get; set; }
        public string Notes { get; set; }        
        public Project Project { get; set; }

        
    }
}