using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProjects.Models
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