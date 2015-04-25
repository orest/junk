using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProjects.Models
{
    public class Client
    {
        public Client()
        {
            Projects = new List<Project>();
        }
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Rate { get; set; }
        public string TimesheetCode { get; set; }
        public string TimesheetFrequencyCode { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        public List<Project> Projects { get; set; }


    }
}