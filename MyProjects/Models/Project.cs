using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProjects.Models
{
    public class Project
    {
        public Project()
        {
            Tasks = new List<Task>();
            TimeSheet = new List<WorkLog>();
        }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int ProjectStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public string Duration { get; set; }
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }
        [Display(Name = "Max Hours/Week")]
        public decimal? MaxHoursPerWeek { get; set; }
        [Display(Name = "Min Hours/Week")]
        public decimal? MinHoursPerWeek { get; set; }
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public List<Task> Tasks { get; set; }
        public Client Client { get; set; }
        public List<WorkLog> TimeSheet { get; set; }

    }
}