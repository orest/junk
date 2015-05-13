
namespace Timetracker.Models
{
    public class ProjectCommand
    {
        public int ProjectId { get; set; }
        public ProjectCommandActions Action { get; set; }
        public string ActionDetails { get; set; }

    }

    public enum ProjectCommandActions
    {
        Start,
        Restart,
        Pause,
        Stop,
        Time
    }
}
